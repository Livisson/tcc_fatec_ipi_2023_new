import React, { useState, useEffect } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import { 
  FaUser, 
  FaChartBar, 
  FaDollyFlatbed, 
  FaFileInvoiceDollar,
  FaClipboardList, 
  FaBox, 
  FaMoneyBillWave, 
  FaCashRegister, 
  FaCog, 
  FaSignOutAlt, 
  FaPencilAlt 
} from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Modal, Form, Toast } from 'react-bootstrap';
import './styleConfig.css';
import axios from "axios";

const Config = () => {

  const [usuario, setUsuario] = useState([]);
  const [itemSelecionado, setItemSelecionado] = useState(null);
  const [modalAberto, setModalAberto] = useState(false);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [formEnviado, setFormEnviado] = useState(false);

  const [nome, setNome] = useState("");
  const [senha, setSenha] = useState("");
  const [email, setEmail] = useState("");


  function handleNomeChange(event) {
    setNome(event.target.value);
  }

  function handleSenhaChange(event) {
    console.log(event.target.value.length)
    setSenha(event.target.value);
  }

  function handleEmailChange(event) {
    setEmail(event.target.value);
  }

  const getUsuario = () => {
    axios
      .get("https://localhost:44334/api/Usuario")
      .then((response) => {
        setUsuario(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  };

  const handleEditar = (event) => {
    event.preventDefault();
    setFormEnviado(true);

    const usuarioEditado = {
      id: itemSelecionado.id,
      nome: nome,
      senha: senha,
      email: email
    };
  
    axios.put("https://localhost:44334/api/Usuario/", usuarioEditado)
    .then(response => {
      getUsuario();
      setSuccessMessage("Usuário editado com sucesso!")
      setShowSuccessToast(true)
    })
    .catch(error => {
      console.log(error);
      setErrorMessage(error.response.data.error || "Erro ao editar Usuário.")
      setShowErrorToast(true)
    });
  
    setNome("");
    setSenha("");
    setEmail("");
    setItemSelecionado(null);
    setModalAberto(false);
    setFormEnviado(false);
  }
  
  useEffect(() => {
    axios
      .get("https://localhost:44334/api/Usuario")
      .then((response) => {
        setUsuario(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  const userToken = localStorage.getItem("user_token");

  const editarUsuario = (item) => {
    setItemSelecionado(item);
    setNome(item.nome);
    setSenha("");
    setEmail(item.email);
    setModalAberto(true);
    //setModoEditar(true);
  };

  return (
    <Container fluid style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <img className="col-2 p-0" src={LogoCompre} alt="Logo" style={{borderRadius: 7, textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}/>
        <div className="col" style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}>
          <label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>CONSOLIDADO</label>
        </div>
        <div className="col" style={{textAlign: "right", verticalAlign: "middle", alignSelf: "center"}}>
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
                <Dropdown.Item eventKey="1">
                  <Link to="/config" style={{color: 'grey', textDecoration: 'none', display: 'flex', alignItems: 'center'}}>
                    <FaCog  className="me-2" />Configurações
                  </Link>
                </Dropdown.Item>
                <Dropdown.Item eventKey="2">
                  <Link to="/login" style={{color: 'grey', textDecoration: 'none', display: 'flex', alignItems: 'center'}}>
                    <FaSignOutAlt  className="me-2" />Sair
                  </Link>
                </Dropdown.Item>
              </DropdownButton>
            </div>
          </Row>
        </div>
      </Row>
      <Row className="justify-content-md-center">
        <Col style={{backgroundColor: '#f8f9fa'}} xs={2} className="pt-4">
          <Row>
            <Button variant="light" className="custom-button-menu">
              <Link style={{color: 'grey'}} className="nav-link" to="/consolidado">
                <FaChartBar className="me-2" />Consolidado
              </Link>
            </Button>
          </Row>
          <Row>
            <Button variant="light" className="custom-button-menu">
              <Link style={{color: 'grey'}} className="nav-link" to="/despesas">
                <FaFileInvoiceDollar className="me-2" />Despesas
              </Link>
            </Button>
          </Row>
          <Row>
            <Button variant="light" className="custom-button-menu">
              <Link style={{color: 'grey'}} className="nav-link" to="/pedidos">
                <FaClipboardList className="me-2" />Pedidos
              </Link>
            </Button>
          </Row>
          <Row>
            <Button variant="light" className="custom-button-menu">
              <Link style={{color: 'grey'}} className="nav-link" to="/fornecedores">
                <FaDollyFlatbed className="me-2" />Fornecedores
              </Link>
            </Button>
          </Row>
          <Row>
            <Button variant="light" className="custom-button-menu">
              <Link style={{color: 'grey'}} className="nav-link" to="/estoque">
                <FaBox className="me-2" />Estoque
              </Link>
            </Button>
          </Row>
          <Row>
            <Button variant="light" className="custom-button-menu">
              <Link style={{color: 'grey'}} className="nav-link" to="/precificar">
                <FaMoneyBillWave className="me-2" />Precificação
              </Link>
            </Button>
          </Row>
          <Row>
            <Button variant="light" className="custom-button-menu">
              <Link style={{color: 'grey'}} className="nav-link" to="/caixa">
                <FaCashRegister className="me-2" />Caixa
              </Link>
            </Button>
          </Row>
        </Col>
        <Col>
          <Row className="justify-content-md-center">
            <div className="d-flex justify-content-between">
              <label style={{fontWeight: "bold", color: "Green"}}>Alterar Senha</label>
            </div>
          </Row>
          <br/>
          <br/>
          <Row>
            <Table striped hover>
              <thead>
                <tr>
                  <th className="text-left">Usuário</th>
                  <th className="text-center">Senha</th>
                  <th className="text-center">E-mail Casatrado</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                <tr key={usuario.id}>
                  <td style={{ verticalAlign: "middle", textAlign: "left"}}>{usuario.nome}</td>
                  <td style={{ verticalAlign: "middle", textAlign: "center"}}>{usuario.senha}</td>
                  <td style={{ verticalAlign: "middle", textAlign: "center"}}>{usuario.email}</td>
                  <td className="text-center" style={{ verticalAlign: "middle"}}>
                    <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => editarUsuario(usuario)}>
                      <FaPencilAlt />
                    </Button>
                  </td>
                </tr>
              </tbody>
            </Table>
          </Row>
          <br/>
          <Modal show={modalAberto} onHide={() => setModalAberto(false)}>
            <Modal.Header closeButton>
              <Modal.Title style={{fontWeight: "bold", color: "Grey"}}>{itemSelecionado ? "Editar Usuario" : "Novo Usuário"}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <Form onSubmit={handleEditar}>
                <Form.Group controlId="nome" style={{marginBottom: "20px"}}>
                  <Form.Label>Nome do Usuário</Form.Label>
                  <Form.Control type="text" placeholder="Digite o nome do Usuário" value={nome} onChange={handleNomeChange} required
                    isInvalid={formEnviado && nome.length < 5}/>
                </Form.Group>
                <Form.Group controlId="senha" style={{marginBottom: "20px"}}>
                  <Form.Label>Senha</Form.Label>
                  <Form.Control type="password" placeholder="Digite a nova senha" value={senha} onChange={handleSenhaChange} required minLength={5}
                    isInvalid={formEnviado && senha.length < 5}/>
                </Form.Group>
                <Form.Group controlId="email" style={{marginBottom: "20px"}}>
                  <Form.Label>Quantidade</Form.Label>
                  <Form.Control type="email" placeholder="Digite o e-mail" value={email} onChange={handleEmailChange} required
                    isInvalid={formEnviado && email.length < 8}/>
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
        </Col>
      </Row> 
    </Container>
  );
};

export default Config;
