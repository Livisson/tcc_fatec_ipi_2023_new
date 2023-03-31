import React, { useState, useEffect } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import { FaUser, FaChartBar, FaMapMarkedAlt, FaClipboardList, FaBox, FaMoneyBillWave, FaCashRegister, FaCog, FaSignOutAlt, FaTrash, FaPencilAlt } from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Modal, Form, Toast } from 'react-bootstrap';
import './styleDespesa.css';
import axios from "axios";

const Despesas = () => {

  const [despesas, setDespesas] = useState([]);
  const [despesasGerais, setDespesasGerais] = useState([]);
  const [itemSelecionado, setItemSelecionado] = useState(null);
  const [modalAberto, setModalAberto] = useState(false);
  const [modoEditar, setModoEditar] = useState(false);
  const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  const [itemToDelete, setItemToDelete] = useState(null);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [formEnviado, setFormEnviado] = useState(false);

  const [descricao, setDescricao] = useState("");
  const [funcao, setFuncao] = useState("");
  const [valor, setValor] = useState("");
  const [diaVencimento, setDiaVencimento] = useState("");
  const [tipoDespesa, setTipoDespesa] = useState("Funcionário");

  function handleDescricaoChange(event) {
    setDescricao(event.target.value);
  }

  function handleFuncaoChange(event) {
    setFuncao(event.target.value);
  }

  function handleValorChange(event) {
    setValor(event.target.value);
  }

  function handleDiaVencimentoChange(event) {
    const value = event.target.value.trim();
    console.log(value.toString())
    if (value === '' || (parseInt(value) >= 1 && parseInt(value) <= 31 && !value.toString().includes(","))) {
      setDiaVencimento(value === '' ? null : parseInt(value));
    }
  }

  function getDespesas() {
    axios.get('https://localhost:44334/Despesa')
    .then(response => {
      setDespesas(response.data.filter(despesa => despesa.tipo !== "Geral"));
      setDespesasGerais(response.data.filter(despesa => despesa.tipo === "Geral"));
    })
    .catch(error => {
      console.log(error);
    });
  }

  const handleAdicionar = (event) => {
    event.preventDefault();
    setFormEnviado(true);

    if (event.currentTarget.checkValidity()) {
      const novaDespesa = {
        tipo: tipoDespesa,
        descricao: descricao,
        funcao: funcao,
        valor: parseFloat(valor),
        diaVencimento: parseInt(diaVencimento),
      };

      axios.post("https://localhost:44334/Despesa/", novaDespesa)
      .then(response => {
        getDespesas();
        setSuccessMessage("Despesa Inserida com Sucesso!")
        setShowSuccessToast(true)
      })
      .catch(error => {
        console.log(error);
        setErrorMessage(error.message || "Erro ao salvar despesa.")
        setShowErrorToast(true)
      });

      setDescricao("");
      setFuncao("");
      setValor("");
      setDiaVencimento("");
      setTipoDespesa("");
      setModalAberto(false);
      setFormEnviado(false);
    }
  }

  const handleEditar = (event) => {
    event.preventDefault();
    setFormEnviado(true);

    if (event.currentTarget.checkValidity()) {
      const despesaEditada = {
        id: itemSelecionado.id,
        tipo: tipoDespesa,
        descricao: descricao,
        funcao: funcao,
        valor: parseFloat(valor),
        diaVencimento: parseInt(diaVencimento),
      };
    
      axios.put("https://localhost:44334/Despesa/", despesaEditada)
      .then(response => {
        getDespesas();
        setSuccessMessage("Despesa editada com sucesso!")
        setShowSuccessToast(true)
      })
      .catch(error => {
        console.log(error);
        setErrorMessage(error.message || "Erro ao editar despesa.")
        setShowErrorToast(true)
      });
    
      setDescricao("");
      setFuncao("");
      setValor("");
      setDiaVencimento("");
      setTipoDespesa("");
      setItemSelecionado(null);
      setModalAberto(false);
      setFormEnviado(false);
    }
  }
  
  function handleCloseDeleteConfirmation(confirmed) {
    if (confirmed) {

      axios.delete(`https://localhost:44334/Despesa/${itemToDelete.id}`)
        .then(response => {
          getDespesas();
          setSuccessMessage("Despesa excluída com sucesso!")
          setShowSuccessToast(true)
        })
        .catch(error => {
          console.log(error);
          setErrorMessage(error.message || "Erro ao excluir despesa.")
          setShowErrorToast(true)
        });
    }
    setShowDeleteConfirmation(false);
    setItemToDelete(null);
  }
  
  useEffect(() => {
    axios.get('https://localhost:44334/Despesa')
      .then(response => {
        setDespesas(response.data.filter(despesa => despesa.tipo !== "Geral"));
        setDespesasGerais(response.data.filter(despesa => despesa.tipo === "Geral"));
      })
      .catch(error => {
        console.log(error);
      });
      //getDespesas();
  }, []);

  const userToken = localStorage.getItem("user_token");

  const adicionarDespesa = () => {
    setDescricao("");
    setFuncao("");
    setValor("");
    setDiaVencimento("");
    setTipoDespesa("Funcionário");
    setItemSelecionado(null);
    setModalAberto(true);
    setModoEditar(false);
  };

  const editarDespesa = (item) => {
    console.log(item.tipo)
    setItemSelecionado(item);
    setDescricao(item.descricao);
    setFuncao(item.funcao);
    setValor(item.valor);
    setDiaVencimento(item.diaVencimento);
    setTipoDespesa(item.tipo);
    setModalAberto(true);
    setModoEditar(true);
  };

  const removerDespesa = (item) => {
    setItemToDelete(item);
    setShowDeleteConfirmation(true);
    /*
    axios.delete(`https://localhost:44334/Despesa/${id}`)
      .then(response => {
        const novasDespesas = despesas.filter(despesa => despesa.id !== id);
        setDespesas(novasDespesas);
      })
      .catch(error => {
        console.log(error);
      });*/
  };

  return (
    <Container style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}>
          <img src={LogoCompre} alt="Logo" height="80" style={{borderRadius: 7}}/>
        </Col>
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}><label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>MAPA DE CUSTO</label></Col>
        <Col style={{textAlign: "right", verticalAlign: "middle", alignSelf: "center"}}>
          <Row style={{ height: '50px'}}>
            <div className="mb-2">
              <DropdownButton
                key="start"
                id={`dropdown-button-drop-start`}
                drop="start"
                variant="outline-secondary"
                title={
                  <>
                    <span style={{marginLeft: "10px", marginRight: "10px"}}>{JSON.parse(userToken).name}</span>
                    <FaUser className="me-2" />
                  </>
                }
              >
                <Dropdown.Item eventKey="1"><Link to="/config" style={{color: 'grey', textDecoration: 'none', display: 'flex', alignItems: 'center'}}><FaCog  className="me-2" />Configurações</Link></Dropdown.Item>
                <Dropdown.Item eventKey="2"><Link to="/" style={{color: 'grey', textDecoration: 'none', display: 'flex', alignItems: 'center'}}><FaSignOutAlt  className="me-2" />Sair</Link></Dropdown.Item>
              </DropdownButton>
            </div>
          </Row>
        </Col>
      </Row>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex justify-content-between">
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/consolidado"><FaChartBar className="me-2" />Consolidado</Link></Button>
          <Button variant="light" className="custom-button-menu-selected"><Link style={{color: 'grey'}} className="nav-link" to="/despesas"><FaMapMarkedAlt className="me-2" />Mapa de Custos</Link></Button>
          <Dropdown className="d-inline-block">
            <Dropdown.Toggle style={{color: 'grey'}} className="custom-button-menu" variant="light" id="dropdown-basic">
              <FaClipboardList className="me-2" />Pedidos
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item style={{color: 'grey'}}><Link style={{color: 'grey'}} className="nav-link" to="/pedidos">Pedidos</Link></Dropdown.Item>
              <Dropdown.Item style={{color: 'grey'}}><Link style={{color: 'grey'}} className="nav-link" to="/fornecedores">Fornecedores</Link></Dropdown.Item>
              {/*<Dropdown.Item style={{color: 'grey'}}><Link style={{color: 'grey'}} className="nav-link" to="/produtos">Produtos</Link></Dropdown.Item>*/}
            </Dropdown.Menu>
          </Dropdown>
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/estoque"><FaBox className="me-2" />Estoque</Link></Button>
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/precificar"><FaMoneyBillWave className="me-2" />Precificação</Link></Button>
          <Button variant="light" className="custom-button-menu-last"><Link style={{color: 'grey'}} className="nav-link" to="/caixa"><FaCashRegister className="me-2" />Caixa</Link></Button>
        </div>
      </Row>
      <br/>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex justify-content-between">
          <label style={{fontWeight: "bold", color: "Green"}}>Despesas Fixas</label>
          <Button variant="warning" className="custom-button-add" style={{ height: "35px", width: "100px", marginBottom: "5px", color:"grey" }} onClick={() => adicionarDespesa()}>Adicionar</Button>
        </div>
      </Row>
      <Table striped hover>
        <thead>
          <tr>
            <th className="text-center">N°</th>
            <th>Funcionários</th>
            <th>Função</th>
            <th className="text-center">Valor</th>
            <th className="text-center">Dia Pagamento</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {despesas.map((item, index) => (
            <tr key={item.id}>
              <td className="text-center" style={{ verticalAlign: "middle"}}>{index}</td>
              <td style={{ verticalAlign: "middle"}}>{item.descricao}</td>
              <td style={{ verticalAlign: "middle"}}>{item.funcao}</td>
              <td className="text-center" style={{ verticalAlign: "middle"}}>R$ {item.valor.toFixed(2)}</td>
              <td className="text-center" style={{ verticalAlign: "middle"}}>{item.diaVencimento}</td>
              <td className="text-center" style={{ verticalAlign: "middle"}}>
                <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => editarDespesa(item)}>
                  <FaPencilAlt />
                </Button>
                <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => removerDespesa(item)}>
                  <FaTrash />
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
      <br/>
      <Table striped hover>
        <thead>
          <tr>
            <th className="text-center">N°</th>
            <th>Geral</th>
            <th></th>
            <th className="text-center">Valor</th>
            <th className="text-center">Dia Pagamento</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {despesasGerais.map((item, index) => (
            <tr key={item.id}>
              <td className="text-center" style={{ verticalAlign: "middle"}}>{index}</td>
              <td style={{ verticalAlign: "middle"}}>{item.descricao}</td>
              <td></td>
              <td className="text-center" style={{ verticalAlign: "middle"}}>R$ {item.valor.toFixed(2)}</td>
              <td className="text-center" style={{ verticalAlign: "middle"}}>{item.diaVencimento}</td>
              <td className="text-center" style={{ verticalAlign: "middle"}}>
                <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => editarDespesa(item)}>
                  <FaPencilAlt />
                </Button>
                <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => removerDespesa(item)}>
                  <FaTrash />
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
      <Modal show={modalAberto} onHide={() => setModalAberto(false)}>
        <Modal.Header closeButton>
          <Modal.Title style={{fontWeight: "bold", color: "Grey"}}>{itemSelecionado ? "Editar Despesa" : "Nova Despesa"}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={modoEditar ? handleEditar : handleAdicionar}>
            <Form.Group controlId="tipoDespesa" style={{marginBottom: "20px"}}>
              <Form.Label>Tipo de Despesa</Form.Label>
              <Form.Select value={tipoDespesa} onChange={(e) => setTipoDespesa(e.target.value)} required isInvalid={formEnviado && tipoDespesa.length === 0}>
                <option value="Funcionário">Funcionário</option>
                <option value="Geral">Geral</option>
              </Form.Select>
            </Form.Group>
            <Form.Group controlId="descricao" style={{marginBottom: "20px"}}>
              <Form.Label>Descrição</Form.Label>
              <Form.Control type="text" placeholder="Digite o nome da despesa" value={descricao} onChange={handleDescricaoChange} required isInvalid={formEnviado && descricao.length === 0}/>
            </Form.Group>
            <Form.Group controlId="funcao" style={{marginBottom: "20px"}}>
              <Form.Label>Função</Form.Label>
              <Form.Control type="text" placeholder="Digite a função do despesa" value={funcao} onChange={handleFuncaoChange} disabled={tipoDespesa === "Geral"} required isInvalid={formEnviado && funcao.length === 0}/>
            </Form.Group>
            <Form.Group controlId="valor" style={{marginBottom: "20px"}}>
              <Form.Label>Valor</Form.Label>
              <Form.Control type="number" step="0.01" placeholder="Digite o valor da despesa" value={valor} onChange={handleValorChange} required isInvalid={formEnviado && valor.length === 0}/>
            </Form.Group>
            <Form.Group controlId="data" style={{marginBottom: "20px"}}>
              <Form.Label>Dia Pagamento</Form.Label>
              <Form.Control type="number" pattern="[0-9]*" min="1" max="31" placeholder="Digite o dia de vencimento" value={diaVencimento} onChange={handleDiaVencimentoChange} required isInvalid={formEnviado && diaVencimento.length === 0}/>
            </Form.Group>
            <Modal.Footer>
              <Button variant="success" type="submit">
                Salvar
              </Button>
              <Button variant="secondary" onClick={() => setModalAberto(false)}>Fechar</Button>
            </Modal.Footer>
          </Form>
        </Modal.Body>  
      </Modal>
      {showDeleteConfirmation && (
        <Modal show={showDeleteConfirmation} onHide={() => handleCloseDeleteConfirmation(false)}>
          <Modal.Header closeButton>
            <Modal.Title>Confirmação de exclusão</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Tem certeza que deseja excluir a despesa "{itemToDelete.descricao}"?
          </Modal.Body>
          <Modal.Footer>
            <Button variant="danger" onClick={() => handleCloseDeleteConfirmation(true)}>
              Confirmar
            </Button>
            <Button variant="secondary" onClick={() => handleCloseDeleteConfirmation(false)}>
              Cancelar
            </Button>
          </Modal.Footer>
        </Modal>
      )}
      <Toast show={showErrorToast} onClose={() => setShowErrorToast(false)} bg="danger" delay={3000} autohide>
        <Toast.Body className="text-white">{errorMessage}</Toast.Body>
      </Toast>
      <Toast show={showSuccessToast} onClose={() => setShowSuccessToast(false)} bg="success" delay={3000} autohide>
        <Toast.Body className="text-white">{successMessage}</Toast.Body>
      </Toast>  
    </Container>
  );
};

export default Despesas;
