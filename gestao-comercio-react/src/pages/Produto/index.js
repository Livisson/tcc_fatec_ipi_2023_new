import React, { useState, useEffect } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import { FaUser, FaChartBar, FaMapMarkedAlt, FaClipboardList, FaBox, FaMoneyBillWave, FaCashRegister, FaCog, FaSignOutAlt, FaTrash, FaPencilAlt } from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Modal, Form, Toast } from 'react-bootstrap';
import './styleProduto.css';
import axios from "axios";

const Produto = () => {

  const [nomeProdutos, setNomeProdutos] = useState([]);
  const [itemSelecionado, setItemSelecionado] = useState(null);
  const [modalAberto, setModalAberto] = useState(false);
  const [modoEditar, setModoEditar] = useState(false);
  const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  const [itemToDelete, setItemToDelete] = useState(null);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");

  const [nome, setNome] = useState("");

  function handleNomeChange(event) {
    setNome(event.target.value);
  }

  function getNomeProdutos() {
    axios.get('https://localhost:44334/NomeProdutos')
    .then(response => {
      setNomeProdutos(response.data);
    })
    .catch(error => {
      console.log(error);
    });
  }

  const handleAdicionar = (event) => {
    event.preventDefault();


    const novoNomeProduto = {
      nomeProduto: nome
    };

    axios.post("https://localhost:44334/NomeProdutos/", novoNomeProduto)
    .then(response => {
      getNomeProdutos();
      setSuccessMessage("Nome de Produto inserido com Sucesso!")
      setShowSuccessToast(true)
    })
    .catch(error => {
      console.log(error);
      setErrorMessage(error.response.data.error || "Erro ao salvar Nome de Produto.")
      setShowErrorToast(true)
    });

    setNome("");
    setModalAberto(false);
  }

  const handleEditar = (event) => {
    event.preventDefault();

    console.log(nomeProdutos)
    const nomeProdutoEditado = {
      id: itemSelecionado.id,
      nomeProduto: nome,
    };
  
    axios.put("https://localhost:44334/NomeProdutos/", nomeProdutoEditado)
    .then(response => {
      getNomeProdutos();
      setSuccessMessage("Nome de Produto editado com sucesso!")
      setShowSuccessToast(true)
    })
    .catch(error => {
      console.log(error);
      setErrorMessage(error.response.data.error || "Erro ao editar Nome de Produto.")
      setShowErrorToast(true)
    });
  
    setNome("");
    setItemSelecionado(null);
    setModalAberto(false);
  }
  
  function handleCloseDeleteConfirmation(confirmed) {
    if (confirmed) {

      axios.delete(`https://localhost:44334/NomeProdutos/${itemToDelete.id}`)
        .then(response => {
          getNomeProdutos();
          setSuccessMessage("Nome de Produto excluído com sucesso!")
          setShowSuccessToast(true)
        })
        .catch(error => {
          console.log(error);
          setErrorMessage(error.message || "Erro ao excluir Nome de Produto.")
          setShowErrorToast(true)
        });
    }
    setShowDeleteConfirmation(false);
    setItemToDelete(null);
  }
  
  useEffect(() => {
    axios.get('https://localhost:44334/NomeProdutos')
      .then(response => {
        setNomeProdutos(response.data);
      })
      .catch(error => {
        console.log(error);
      });
  }, []);

  const userToken = localStorage.getItem("user_token");

  const adicionarNomeProduto = () => {
    setNome("");
    setModalAberto(true);
    setModoEditar(false);
  };

  const editarNomeProduto = (item) => {
    setItemSelecionado(item);
    setNome(item.nomeProduto);
    setModalAberto(true);
    setModoEditar(true);
  };

  const removerNomeProduto = (item) => {
    setItemToDelete(item);
    setShowDeleteConfirmation(true);
  };

  return (
    <Container style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}>
          <img src={LogoCompre} alt="Logo" height="80" style={{borderRadius: 7}}/>
        </Col>
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}><label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>PRODUTOS</label></Col>
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
              <FaClipboardList className="me-2" />Produtos
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item style={{color: 'grey'}}><Link style={{color: 'grey'}} className="nav-link" to="/pedidos">Pedidos</Link></Dropdown.Item>
              <Dropdown.Item style={{color: 'grey'}}><Link style={{color: 'grey'}} className="nav-link" to="/fornecedores">Fornecedores</Link></Dropdown.Item>
              <Dropdown.Item style={{color: 'grey'}}>Produtos</Dropdown.Item>
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
          <label style={{fontWeight: "bold", color: "Green"}}>Produtos</label>
          <Button variant="warning" className="custom-button-add" style={{ height: "35px", width: "100px", marginBottom: "5px", color:"grey" }} onClick={() => adicionarNomeProduto()}>Adicionar</Button>
        </div>
      </Row>
      <Row>
        <Table striped hover>
          <thead>
            <tr>
              <th className="text-center">Nome Produtos</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {nomeProdutos.map((item, index) => (
              <tr key={item.id}>
                <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.nomeProduto}</td>
                <td className="text-center" style={{ verticalAlign: "middle"}}>
                  <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => editarNomeProduto(item)}>
                    <FaPencilAlt />
                  </Button>
                  <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => removerNomeProduto(item)}>
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
          <Modal.Title style={{fontWeight: "bold", color: "Grey"}}>{itemSelecionado ? "Editar Nome Produto" : "Novo Nome de Produto"}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={modoEditar ? handleEditar : handleAdicionar}>
            <Form.Group controlId="nome" style={{marginBottom: "20px"}}>
              <Form.Label>Nome</Form.Label>
              <Form.Control type="text" placeholder="Digite o nome do produto" value={nome} onChange={handleNomeChange} />
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
            Tem certeza que deseja excluir esse nome de produto?"{itemToDelete.nome}"?
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

export default Produto;