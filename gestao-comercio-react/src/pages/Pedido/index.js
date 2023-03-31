import React, { useState, useEffect, useCallback } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import { FaUser, FaChartBar, FaMapMarkedAlt, FaClipboardList, FaBox, FaMoneyBillWave, FaCashRegister, FaCog, FaSignOutAlt, FaTrash } from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Modal, Form, Toast } from 'react-bootstrap';
import './stylePedido.css';
import axios from "axios";

const Pedido = () => {

  const [pedido, setPedido] = useState([]);
  //const [itemSelecionado, setItemSelecionado] = useState(null);
  const [modalAberto, setModalAberto] = useState(false);
  //const [modoEditar, setModoEditar] = useState(false);
  const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  const [itemToDelete, setItemToDelete] = useState(null);
  const [fornecedores, setFornecedores] = useState([]);
  const [fornecedorFiltro, setFornecedorFiltro] = useState("");
  const [produtos, setProdutos] = useState([]);
  const [disableProd, setDisableProd] = useState(true);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [formEnviado, setFormEnviado] = useState(false);

  const [codigoFornecedor, setCodigoFornecedor] = useState("");
  const [nomeProduto, setNomeProduto] = useState("");
  const [codigoBarras, setCodigoBarras] = useState("");
  const [valorCompra, setValorCompra] = useState("");
  const [quantidade, setQuantidade] = useState("");
  const [dataPagamento, setDataPagamento] = useState("");


  function handleCodigoFornecedorChange(event) {
    setCodigoFornecedor(event.target.value);
  }

  function handleNomeProdutoChange(event) {
    setNomeProduto(event.target.value);
  }

  function handleCodigoBarrasChange(event) {
    setCodigoBarras(event.target.value);
    if (event.target.value.length > 9) {
      var nomeProd = produtos[0] ? produtos[0].find(produto => produto.codigoBarras === event.target.value) : ""
      if(nomeProd !== undefined){
        if (nomeProd.nome !== "") {
          setNomeProduto(nomeProd.nome);
          setDisableProd(true);
        }
        else{
          setNomeProduto("");
          setDisableProd(false);
        }
      }
      else{
        setNomeProduto("");
        setDisableProd(false);
      }
    }

    if(event.target.value === "") {
      setNomeProduto("");
    }
  }

  function handleValorCompraChange(event) {
    setValorCompra(event.target.value);
  }

  function handleQuantidadeChange(event) {
    setQuantidade(event.target.value);
  }

  function handleDataPagamentoChange(event) {
    setDataPagamento(event.target.value);
  }

  const getPedido = useCallback(() => {
    axios.get(`https://localhost:44334/Pedido?codigoFornecedor=${fornecedorFiltro}`)
    .then(response => {
      setPedido(response.data);
    })
    .catch(error => {
      console.log(error);
    });
  }, [fornecedorFiltro]);

  const getProdutos = useCallback(() => {
    axios
      .get("https://localhost:44334/Pedido/getProdutos")
      .then((response) => {
        setProdutos([response.data]);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  const handleAdicionar = (event) => {
    event.preventDefault();
    setFormEnviado(true);

    if (event.currentTarget.checkValidity()) {
      const novoPedido = {
        codigoFornecedor: codigoFornecedor,
        nomeProduto: nomeProduto,
        codigoBarras: codigoBarras,
        valorCompra: valorCompra,
        quantidade: quantidade,
        dataPagamento: dataPagamento
      };

      axios.post("https://localhost:44334/Pedido/", novoPedido)
      .then(response => {
        getPedido();
        getProdutos();
        setSuccessMessage("Pedido inserido com Sucesso!")
        setShowSuccessToast(true)
      })
      .catch(error => {
        console.log(error);
        setErrorMessage("Erro ao salvar Pedido.")
        setShowErrorToast(true)
      });

      setCodigoFornecedor("");
      setNomeProduto("");
      setCodigoBarras("");
      setValorCompra("");
      setQuantidade("");
      setDataPagamento("");
      setModalAberto(false);
      setFormEnviado(false);
    }
  }
  
  function handleCloseDeleteConfirmation(confirmed) {
    if (confirmed) {
      
      const auxItemToDelete = {
        codigoFornecedor: fornecedores ? fornecedores.find(fornecedor => fornecedor.nome === itemToDelete.nomeFornecedor).cnpj : "",
        nomeProduto: itemToDelete.nomeProduto,
        codigoBarras: itemToDelete.codigoBarras,
        valorCompra: itemToDelete.valorCompra,
        quantidade: itemToDelete.quantidade,
        dataPagamento: itemToDelete.dataPagamento
      };

      axios.post("https://localhost:44334/Pedido/deletePedido", auxItemToDelete)
        .then(response => {
          getPedido();
          setSuccessMessage("Pedido excluído com sucesso!")
          setShowSuccessToast(true)
        })
        .catch(error => {
          console.log(error);
          setErrorMessage(error.message || "Erro ao excluir Pedido.")
          setShowErrorToast(true)
        });
    }
    setShowDeleteConfirmation(false);
    setItemToDelete(null);
  }
  
  useEffect(() => {

    if (fornecedorFiltro !== "") {
      getPedido();
    }

    const jsonGetInicial = {
      codigoFornecedor: ""
    };
  
    if (fornecedores.length === 0) {
      axios
        .get("https://localhost:44334/Pedido/", jsonGetInicial)
        .then((response) => {
          setPedido(response.data);
        })
        .catch((error) => {
          console.log(error);
        });
    }

    if (fornecedores.length === 0) {
      axios
        .get("https://localhost:44334/Fornecedor")
        .then((response) => {
          setFornecedores(["", ...response.data]);
        })
        .catch((error) => {
          console.log(error);
        });

        axios
        .get("https://localhost:44334/Pedido/getProdutos")
        .then((response) => {
          setProdutos([response.data]);
        })
        .catch((error) => {
          console.log(error);
        });
    }

  }, [fornecedorFiltro, fornecedores, getPedido]);

  const userToken = localStorage.getItem("user_token");

  const adicionarPedido = () => {
    setCodigoFornecedor("");
    setNomeProduto("");
    setCodigoBarras("");
    setValorCompra("");
    setQuantidade("");
    setDataPagamento("");
    setModalAberto(true);
    //setModoEditar(false);
  };

  /*const editarPedido = (item) => {
    setItemSelecionado(item);
    setCodigoFornecedor(item.codigoFornecedor);
    setNomeProduto(item.nomeProduto);
    setCodigoBarras(item.codigoBarras);
    setValorCompra(item.valorCompra);
    setQuantidade(item.quantidade);
    setDataPagamento(item.dataPagamento);
    setModalAberto(true);
    setModoEditar(true);
  };*/

  const removerPedido = (item) => {
    setItemToDelete(item);
    setShowDeleteConfirmation(true);
  };

  function handleSelectFiltro(selectedKey) {
    setFornecedorFiltro(selectedKey);
    //console.log(fornecedores.find(fornecedor => fornecedor.cnpj === fornecedorFiltro))
  }

  return (
    <Container style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}>
          <img src={LogoCompre} alt="Logo" height="80" style={{borderRadius: 7}}/>
        </Col>
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}><label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>PEDIDOS</label></Col>
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
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/despesas"><FaMapMarkedAlt className="me-2" />Mapa de Custos</Link></Button>
          <Dropdown className="d-inline-block">
            <Dropdown.Toggle style={{color: 'grey'}} className="custom-button-menu-selected" variant="light" id="dropdown-basic">
              <FaClipboardList className="me-2" />Pedidos
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item style={{color: 'grey'}}>Pedidos</Dropdown.Item>
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
          <label style={{fontWeight: "bold", color: "Green"}}>Pedidos</label>
        </div>
      </Row>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex justify-content-between">
          <div className="d-flex align-items-center mb-4">
            <label style={{ flex: 1, marginRight:"10px", fontWeight: "bold", color: "Grey" }}>Fornecedor</label>
            <DropdownButton id="filtro-dropdown" title={fornecedorFiltro ? fornecedores.find(fornecedor => fornecedor.cnpj === fornecedorFiltro).nome  : "Selecione um Fornecedor"} variant="outline-secondary">
              {fornecedores.map((fornecedor) => (
                <Dropdown.Item className="empty-option" key={fornecedor.cnpj} eventKey={fornecedor.cnpj} onClick={handleSelectFiltro.bind(this, fornecedor.cnpj, fornecedor.eventKey)} onSelect={handleSelectFiltro}>
                  {fornecedor.nome}
                </Dropdown.Item>
              ))}
            </DropdownButton>
          </div>
          <Button variant="warning" className="custom-button-add" style={{ height: "35px", width: "100px", marginBottom: "5px", color:"grey" }} onClick={() => adicionarPedido()}>Adicionar</Button>
        </div>
      </Row>
      <Row>
        <Table striped hover>
          <thead>
            <tr>
              <th className="text-center">Produto</th>
              <th className="text-center">Cód. Barras</th>
              <th className="text-center">Fornecedor</th>
              <th className="text-center">Valor R$(un.)</th>
              <th className="text-center">Quant.</th>
              <th className="text-center">Valor R$(total)</th>
              <th className="text-center">Pagamento</th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {pedido.map((item, index) => (
              <tr key={item.id}>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.nomeProduto}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.codigoBarras}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.nomeFornecedor}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>R$ {item.valorCompra.toFixed(2)}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.quantidade}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>R$ {item.valorTotal.toFixed(2)}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{new Date(item.dataPagamento).toLocaleDateString("pt-BR")}</td>
                <td className="text-center" style={{ verticalAlign: "middle"}}>
                  <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => removerPedido(item)}>
                    <FaTrash />
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      </Row>
      <br/>
      <Modal show={modalAberto} onHide={() => setModalAberto(false)}>
        <Modal.Header closeButton>
          <Modal.Title style={{fontWeight: "bold", color: "Grey"}}>{"Novo Pedido"}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleAdicionar}>
            <Form.Group controlId="fornecedor" style={{ marginBottom: "20px" }}>
              <Form.Label>Fornecedor</Form.Label>
              <Form.Control
                as="select"
                value={codigoFornecedor}
                onChange={handleCodigoFornecedorChange}
                required
                isInvalid={formEnviado && codigoFornecedor.length === 0}
              >
                <option value="">Selecione um fornecedor</option>
                {fornecedores.map((fornecedor) => (
                  <option key={fornecedor.cnpj} value={fornecedor.cnpj}>
                    {fornecedor.nome}
                  </option>
                ))}
              </Form.Control>
            </Form.Group>
            <Form.Group controlId="codigoBarras" style={{ marginBottom: "20px" }}>
              <Form.Label>Código de Barras</Form.Label>
              <Form.Control
                type="text"
                placeholder="Digite o código de barras"
                value={codigoBarras}
                onChange={handleCodigoBarrasChange}
                required
                minLength={13}
                isInvalid={formEnviado && codigoBarras.length < 13}
              />
            </Form.Group>
            <Form.Group controlId="produto" style={{ marginBottom: "20px" }}>
              <Form.Label>Nome produto</Form.Label>
              <Form.Control
                type="text"
                placeholder="Digite o nome do produto"
                value={nomeProduto}
                onChange={handleNomeProdutoChange}
                disabled={disableProd}
                required
                isInvalid={formEnviado && nomeProduto.length === 0}
              />
            </Form.Group>
            <Form.Group controlId="valorCompra" style={{ marginBottom: "20px" }}>
              <Form.Label>Valor Compra</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                placeholder="Digite o valor da compra"
                value={valorCompra}
                onChange={handleValorCompraChange}
                required
                isInvalid={formEnviado && valorCompra.length === 0}
              />
            </Form.Group>
            <Form.Group controlId="quantidade" style={{marginBottom: "20px"}}>
              <Form.Label>Quantidade</Form.Label>
              <Form.Control type="number" placeholder="Digite a quantidade" value={quantidade} onChange={handleQuantidadeChange} required isInvalid={formEnviado && quantidade.length === 0}/>
            </Form.Group>
            <Form.Group controlId="dataPagamento" style={{marginBottom: "20px"}}>
              <Form.Label>Data Pagamento</Form.Label>
              <Form.Control type="date" placeholder="Selecione a data de pagamento" value={dataPagamento} onChange={handleDataPagamentoChange} required isInvalid={formEnviado && dataPagamento.length === 0}/>
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
            Tem certeza que deseja excluir o Pedido "{itemToDelete.nomeProduto}"?
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

export default Pedido;