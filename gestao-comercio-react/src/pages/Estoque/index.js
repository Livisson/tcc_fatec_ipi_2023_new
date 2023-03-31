import React, { useState, useEffect, useCallback } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import { FaUser, FaChartBar, FaMapMarkedAlt, FaClipboardList, FaBox, FaMoneyBillWave, FaCashRegister, FaCog, FaSignOutAlt, FaPencilAlt } from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Modal, Form, Toast } from 'react-bootstrap';
import './styleEstoque.css';
import axios from "axios";

const Estoque = () => {

  const [estoque, setEstoque] = useState([]);
  const [itemSelecionado, setItemSelecionado] = useState(null);
  const [modalAberto, setModalAberto] = useState(false);
  //const [modoEditar, setModoEditar] = useState(false);
  //const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  //const [itemToDelete, setItemToDelete] = useState(null);
  const [fornecedores, setFornecedores] = useState([]);
  const [fornecedorFiltro, setFornecedorFiltro] = useState("");
  const [produtos, setProdutos] = useState([]);
  const [produtoFiltro, setProdutoFiltro] = useState("");

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [formEnviado, setFormEnviado] = useState(false);

  const [nome, setNome] = useState("");
  const [nomeFornecedor, setNomeFornecedor] = useState("");
  const [quantidade, setQuantidade] = useState("");
  //const [cnpjFornecedor, setCnpjFornecedor] = useState("");
  //const [codigoBarras, setCodigoBarras] = useState("");

  function handleNomeChange(event) {
    setNome(event.target.value);
  }

  const getProdutos = useCallback(() => {
    console.log("get Produto")
    axios
      .get("https://localhost:44334/Pedido/getProdutos")
      .then((response) => {
        setProdutos(["", ...response.data]);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  const getEstoque = useCallback(() => {
    axios.get(`https://localhost:44334/Pedido/getEstoque?codigoFornecedor=${fornecedorFiltro}&nomeProduto=${produtoFiltro}`)
    .then(response => {
      setEstoque(response.data);
    })
    .catch(error => {
      console.log(error);
    });
  }, [fornecedorFiltro, produtoFiltro]);

  const handleEditar = (event) => {
    event.preventDefault();
    setFormEnviado(true);

    console.log(estoque)
    const nomeProdutoEditado = {
      nomeProduto: nome,
      codigoBarras: itemSelecionado.codigoBarras,
      nomeFornecedor: itemSelecionado.nomeFornecedor,
      cnpjFornecedor: itemSelecionado.cnpjFornecedor,
      quantidade: itemSelecionado.quantidade
    };
  
    axios.put("https://localhost:44334/NomeProdutos/", nomeProdutoEditado)
    .then(response => {
      getProdutos();
      getEstoque();
      setSuccessMessage("Nome de Produto editado com sucesso!")
      setShowSuccessToast(true)
    })
    .catch(error => {
      console.log(error);
      setErrorMessage(error.response.data.error || "Erro ao editar Nome de Produto.")
      setShowErrorToast(true)
    });
  
    setNome("");
    setNomeFornecedor("");
    setQuantidade("");
    //setCnpjFornecedor("");
    //setCodigoBarras("");
    setItemSelecionado(null);
    setModalAberto(false);
    setFormEnviado(false);
  }
  
  useEffect(() => {

    if (fornecedorFiltro !== "") {
      getEstoque();
    }

    if (produtoFiltro !== "") {
      getEstoque();
    }

    if (fornecedores.length === 0) {
      const jsonGetInicial = {
        codigoFornecedor: "",
        nomeProduto: ""
      };

      axios.get('https://localhost:44334/Pedido/getEstoque', jsonGetInicial)
        .then(response => {
          setEstoque(response.data);
        })
        .catch(error => {
          console.log(error);
        });


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
        setProdutos(["", ...response.data]);
      })
      .catch((error) => {
        console.log(error);
      });
    }

  }, [fornecedorFiltro, fornecedores, produtos, produtoFiltro, getEstoque]);

  const userToken = localStorage.getItem("user_token");

  const editarNomeProduto = (item) => {
    setItemSelecionado(item);
    setNome(item.nomeProduto);
    setNomeFornecedor(item.nomeFornecedor);
    setQuantidade(item.quantidade);
    //(item.cnpjFornecedor);
    //setCodigoBarras(item.codigoBarras);
    setModalAberto(true);
    //setModoEditar(true);
  };

  function handleSelectFiltroFornecedor(selectedKey) {
    console.log(selectedKey)
    setFornecedorFiltro(selectedKey);
    //console.log(fornecedores.find(fornecedor => fornecedor.cnpj === fornecedorFiltro))
  }

  function handleSelectFiltroProduto(selectedKey) {
    console.log(selectedKey)
    setProdutoFiltro(selectedKey);
    //console.log(fornecedores.find(fornecedor => fornecedor.cnpj === fornecedorFiltro))
  }

  return (
    <Container style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}>
          <img src={LogoCompre} alt="Logo" height="80" style={{borderRadius: 7}}/>
        </Col>
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}><label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>ESTOQUE</label></Col>
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
              <FaClipboardList className="me-2" />Produtos
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item style={{color: 'grey'}}><Link style={{color: 'grey'}} className="nav-link" to="/pedidos">Pedidos</Link></Dropdown.Item>
              <Dropdown.Item style={{color: 'grey'}}><Link style={{color: 'grey'}} className="nav-link" to="/fornecedores">Fornecedores</Link></Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
          <Button variant="light" className="custom-button-menu-selected"><Link style={{color: 'grey'}} className="nav-link" to="/estoque"><FaBox className="me-2" />Estoque</Link></Button>
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/precificar"><FaMoneyBillWave className="me-2" />Precificação</Link></Button>
          <Button variant="light" className="custom-button-menu-last"><Link style={{color: 'grey'}} className="nav-link" to="/caixa"><FaCashRegister className="me-2" />Caixa</Link></Button>
        </div>
      </Row>
      <br/>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex justify-content-between">
          <label style={{fontWeight: "bold", color: "Green"}}>Estoque</label>
        </div>
      </Row>
      <br/>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex">
          <div className="d-flex align-items-center mb-4" style={{marginRight:"35px"}}>
            <label style={{ flex: 1, marginRight:"10px", fontWeight: "bold", color: "Grey" }}>Fornecedor</label>
            <DropdownButton id="filtro-dropdown" title={fornecedorFiltro ? fornecedores.find(fornecedor => fornecedor.cnpj === fornecedorFiltro).nome  : "Selecione um Fornecedor"} variant="outline-secondary">
              {fornecedores.map((fornecedor) => (
                <Dropdown.Item className="empty-option" key={fornecedor.cnpj} eventKey={fornecedor.cnpj} onClick={handleSelectFiltroFornecedor.bind(this, fornecedor.cnpj, fornecedor.eventKey)} onSelect={handleSelectFiltroFornecedor}>
                  {fornecedor.nome}
                </Dropdown.Item>
              ))}
            </DropdownButton>
          </div>
          <div className="d-flex align-items-center mb-4">
            <label style={{ flex: 1, marginRight:"10px", fontWeight: "bold", color: "Grey" }}>Produto</label>
            <DropdownButton id="filtro-dropdown" title={produtoFiltro ? produtos.find(produto => produto.nome === produtoFiltro).nome  : "Selecione um Produto"} variant="outline-secondary">
              {produtos.map((produto) => (
                <Dropdown.Item className="empty-option" key={produto.nome} eventKey={produto.nome} onClick={handleSelectFiltroProduto.bind(this, produto.nome, produto.eventKey)} onSelect={handleSelectFiltroProduto}>
                  {produto.nome}
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
              <th className="text-left">Nome Produto</th>
              <th className="text-left">Nome Fornecedor</th>
              <th className="text-center">Quantidade</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {estoque.map((item, index) => (
              <tr key={item.id}>
                <td style={{ verticalAlign: "middle", textAlign: "left"}}>{item.nomeProduto}</td>
                <td style={{ verticalAlign: "middle", textAlign: "left"}}>{item.nomeFornecedor}</td>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.quantidade}</td>
                <td className="text-center" style={{ verticalAlign: "middle"}}>
                  <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => editarNomeProduto(item)}>
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
          <Modal.Title style={{fontWeight: "bold", color: "Grey"}}>{itemSelecionado ? "Editar Nome Produto" : "Novo Nome de Produto"}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleEditar}>
            <Form.Group controlId="fornecedor" style={{marginBottom: "20px"}}>
              <Form.Label>Fornecedor</Form.Label>
              <Form.Control type="text" placeholder="Digite o nome do Fornecedor" value={nomeFornecedor} disabled />
            </Form.Group>
            <Form.Group controlId="nome" style={{marginBottom: "20px"}}>
              <Form.Label>Nome</Form.Label>
              <Form.Control type="text" placeholder="Digite o nome do produto" value={nome} onChange={handleNomeChange} required
                isInvalid={formEnviado && nome.length < 5}/>
            </Form.Group>
            <Form.Group controlId="quantidade" style={{marginBottom: "20px"}}>
              <Form.Label>Quantidade</Form.Label>
              <Form.Control type="text" placeholder="Digite a quantidade" value={quantidade} disabled />
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

export default Estoque;