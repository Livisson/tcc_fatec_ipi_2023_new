import React, { useState, useEffect, useCallback } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import { FaUser, FaChartBar, FaMapMarkedAlt, FaClipboardList, FaBox, FaMoneyBillWave, FaCashRegister, FaCog, FaSignOutAlt, FaPencilAlt } from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Modal, Form, Toast } from 'react-bootstrap';
import './stylePrecificar.css';
import axios from "axios";

const Precificar = () => {

  const [precificacoes, setPrecificacoes] = useState([]);
  const [itemSelecionado, setItemSelecionado] = useState(null);
  const [modalAberto, setModalAberto] = useState(false);
  //const [modoEditar, setModoEditar] = useState(false);
  // const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  // const [itemToDelete, setItemToDelete] = useState(null);
  const [fornecedores, setFornecedores] = useState([]);
  const [fornecedorFiltro, setFornecedorFiltro] = useState("");
  // const [produtos, setProdutos] = useState([]);
  // const [disableProd, setDisableProd] = useState(true);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [formEnviado, setFormEnviado] = useState(false);

  const [nomeProduto, setNomeProduto] = useState("");
  const [codigoBarras, setCodigoBarras] = useState("");
  //const [codigoFornecedor, setCodigoFornecedor] = useState("");
  const [nomeFornecedor, setNomeFornecedor] = useState("");
  const [valorCompra, setValorCompra] = useState("");
  const [estoque, setEstoque] = useState("");
  const [perDesconto, setPerDesconto] = useState("");
  const [perMargem, setPerMargem] = useState("");
  const [valorSugerido, setValorSugerido] = useState("");
  const [valorVenda, setValorVenda] = useState("");


  function handlePerDescontoChange(event) {
    setPerDesconto(event.target.value);
  }

  function handlePerMargemChange(event) {
    setPerMargem(event.target.value);
  }

  function handleValorVendaChange(event) {
    setValorVenda(event.target.value);
  }

  const getPrecificacao = useCallback(() => {
    axios.get(`https://localhost:44334/Precificacao?codigoFornecedor=${fornecedorFiltro}`)
    .then(response => {
      setPrecificacoes(response.data);
    })
    .catch(error => {
      console.log(error);
    });
  }, [fornecedorFiltro]);

  const handleEditar = (event) => {
    event.preventDefault();
    setFormEnviado(true);

    
    const precificacaoEditado = {
      nomeProduto: itemSelecionado.nomeProduto,
      codigoBarras: itemSelecionado.codigoBarras,
      codigoFornecedor: itemSelecionado.codigoFornecedor,
      valorCompra: itemSelecionado.valorCompra,
      estoque: itemSelecionado.estoque,
      perDesconto: perDesconto,
      perMargem: perMargem,
      valorSugerido: itemSelecionado.valorSugerido,
      valorVenda: valorVenda
    };

    axios.put("https://localhost:44334/Precificacao/", precificacaoEditado)
    .then(response => {
      getPrecificacao();
      setSuccessMessage("Precificação editada com sucesso!")
      setShowSuccessToast(true)
    })
    .catch(error => {
      console.log(error);
      setErrorMessage(error.response.data.error || "Erro ao editar Precificação.")
      setShowErrorToast(true)
    });
  
    setNomeProduto("");
    setCodigoBarras("");
    //setCodigoFornecedor("");
    setNomeFornecedor("");
    setValorCompra("");
    setEstoque("");
    setPerDesconto("");
    setPerMargem("");
    setValorSugerido("");
    setValorVenda("");
    setItemSelecionado(null);
    setModalAberto(false);
    setFormEnviado(false);
  }
  
  
  useEffect(() => {

    if (fornecedorFiltro !== "") {
      console.log(fornecedorFiltro)
      getPrecificacao();
    }

    if (fornecedores.length === 0) {
      axios
        .get("https://localhost:44334/Fornecedor")
        .then((response) => {
          setFornecedores(response.data);
        })
        .catch((error) => {
          console.log(error);
        });
    }

    if (fornecedores.length > 0 && fornecedorFiltro === "") {
      const primeiroFornecedor = fornecedores[0];
      if (primeiroFornecedor && primeiroFornecedor.cnpj) {
        setFornecedorFiltro(primeiroFornecedor.cnpj);
      }
    }

  }, [fornecedorFiltro, fornecedores, getPrecificacao]);

  const userToken = localStorage.getItem("user_token");

  const editarPrecificacao = (item) => {
    setItemSelecionado(item);
    setNomeProduto(item.nomeProduto);
    setCodigoBarras(item.codigoBarras);
    //setCodigoFornecedor(item.codigoFornecedor);
    setNomeFornecedor(item.nomeFornecedor);
    setValorCompra(item.valorCompra);
    setEstoque(item.estoque);
    setPerDesconto(item.perDesconto);
    setPerMargem(item.perMargem);
    setValorSugerido(item.valorSugerido);
    setValorVenda(item.valorVenda);
    setModalAberto(true);
    //setModoEditar(true);
  };

  function handleSelectFiltro(selectedKey) {
    //console.log(selectedKey)
    setFornecedorFiltro(selectedKey);
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
          <Button variant="light" className="custom-button-menu-selected"><Link style={{color: 'grey'}} className="nav-link" to="/precificar"><FaMoneyBillWave className="me-2" />Precificação</Link></Button>
          <Button variant="light" className="custom-button-menu-last"><Link style={{color: 'grey'}} className="nav-link" to="/caixa"><FaCashRegister className="me-2" />Caixa</Link></Button>
        </div>
      </Row>
      <br/>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex justify-content-between">
          <label style={{fontWeight: "bold", color: "Green"}}>Precificação</label>
        </div>
      </Row>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex justify-content-between">
          <div className="d-flex align-items-center mb-4">
            <label style={{ flex: 1, marginRight:"10px", fontWeight: "bold", color: "Grey" }}>Fornecedor</label>
            <DropdownButton id="filtro-dropdown" title={fornecedorFiltro && fornecedores.find(fornecedor => fornecedor.cnpj === fornecedorFiltro)?.nome ? fornecedores.find(fornecedor => fornecedor.cnpj === fornecedorFiltro).nome : "Selecione um Fornecedor"} variant="outline-secondary">
              {fornecedores.map((fornecedor) => (
                <Dropdown.Item className="empty-option" key={fornecedor.cnpj} eventKey={fornecedor.cnpj} onClick={handleSelectFiltro.bind(this, fornecedor.cnpj, fornecedor.eventKey)} onSelect={handleSelectFiltro}>
                  {fornecedor.nome}
                </Dropdown.Item>
              ))}
            </DropdownButton>
          </div>
        </div>
      </Row>
      <Row>
        <Table striped hover>
          <thead>
            <tr>
              <th className="text-center">Produto</th>
              <th className="text-center">Cód. Barras</th>
              <th className="text-center">R$ (Compra)</th>
              <th className="text-center">Estoque</th>
              <th className="text-center">Desconto (%)</th>
              <th className="text-center">Margem (%)</th>
              <th className="text-center">R$ (Valor Sugerido)</th>
              <th className="text-center">R$ (Valor Venda)</th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {precificacoes.map((item, index) => (
              <tr key={item.id}>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.nomeProduto}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.codigoBarras}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>R$ {item.valorCompra.toFixed(2)}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.estoque}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center", backgroundColor: "lightcoral"}}>{item.perDesconto} %</td>
                <td style={{ verticalAlign: "middle", textAlign: "center", backgroundColor: "lightgreen"}}>{item.perMargem} %</td>
                <td style={{ verticalAlign: "middle", textAlign: "center", backgroundColor: "lightgreen"}}>R$ {item.valorSugerido.toFixed(2)}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center", backgroundColor: "lime"}}>R$ {item.valorVenda.toFixed(2)}</td>
                <td className="text-center" style={{ verticalAlign: "middle"}}>
                  <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => editarPrecificacao(item)}>
                    <FaPencilAlt />
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
          <Form onSubmit={handleEditar}>
            <Form.Group controlId="nomeFornecedor" style={{ marginBottom: "20px" }}>
              <Form.Label>Nome do Fornecedor</Form.Label>
              <Form.Control
                type="text"
                placeholder="Digite o nome do fornecedor"
                value={nomeFornecedor}
                required
                disabled
                readOnly
                isInvalid={formEnviado && codigoBarras.length < 13}
              />
            </Form.Group>
            <Form.Group controlId="produto" style={{ marginBottom: "20px" }}>
              <Form.Label>Nome produto</Form.Label>
              <Form.Control
                type="text"
                placeholder="Digite o nome do produto"
                value={nomeProduto}
                disabled
                required
                readOnly
                isInvalid={formEnviado && nomeProduto.length === 0}
              />
            </Form.Group>
            <Form.Group controlId="codigoBarras" style={{ marginBottom: "20px" }}>
              <Form.Label>Código de Barras</Form.Label>
              <Form.Control
                type="text"
                placeholder="Digite o código de barras"
                value={codigoBarras}
                required
                disabled
                readOnly
                isInvalid={formEnviado && codigoBarras.length < 13}
              />
            </Form.Group>
            <Form.Group controlId="estoque" style={{marginBottom: "20px"}}>
              <Form.Label>Qauntidade em Estoque</Form.Label>
              <Form.Control type="number" placeholder="Digite a quantidade" value={estoque} disabled required isInvalid={formEnviado && estoque.length === 0}/>
            </Form.Group>
            <Form.Group controlId="valorCompra" style={{ marginBottom: "20px" }}>
              <Form.Label>Valor Compra (R$)</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                placeholder="Digite o valor da compra"
                value={valorCompra}
                required
                disabled
                readOnly
                isInvalid={formEnviado && valorCompra.length === 0}
              />
            </Form.Group>
            <Form.Group controlId="perDesconto" style={{ marginBottom: "20px" }}>
              <Form.Label>Porcentagem de Desconto (%)</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                placeholder="Digite o valor da % de desconto"
                value={perDesconto}
                onChange={handlePerDescontoChange}
                required
                isInvalid={formEnviado && perDesconto.length === 0}
              />
            </Form.Group>
            <Form.Group controlId="perMargem" style={{ marginBottom: "20px" }}>
              <Form.Label>Porcentagem de Margem (%)</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                placeholder="Digite o valor da % de margem"
                value={perMargem}
                onChange={handlePerMargemChange}
                required
                isInvalid={formEnviado && perMargem.length === 0}
              />
            </Form.Group>
            <Form.Group controlId="valorSugerido" style={{ marginBottom: "20px" }}>
              <Form.Label>Valor Sugerido (R$)</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                placeholder="Digite o valor sugerido"
                value={valorSugerido}
                required
                disabled
                readOnly
                isInvalid={formEnviado && valorSugerido.length === 0}
              />
            </Form.Group>
            <Form.Group controlId="valorVenda" style={{ marginBottom: "20px" }}>
              <Form.Label>Valor Venda (R$)</Form.Label>
              <Form.Control
                type="number"
                step="0.01"
                placeholder="Digite o valor para venda deste produto"
                value={valorVenda}
                onChange={handleValorVendaChange}
                required
                isInvalid={formEnviado && valorVenda.length === 0}
              />
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
      <Toast show={showErrorToast} onClose={() => setShowErrorToast(false)} bg="danger" delay={3000} autohide>
        <Toast.Body className="text-white">{errorMessage}</Toast.Body>
      </Toast>
      <Toast show={showSuccessToast} onClose={() => setShowSuccessToast(false)} bg="success" delay={3000} autohide>
        <Toast.Body className="text-white">{successMessage}</Toast.Body>
      </Toast>  
    </Container>
  );
};

export default Precificar;