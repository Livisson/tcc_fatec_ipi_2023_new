import React, { useState, useEffect } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import InputMask from 'react-input-mask';
import { 
  FaUser,
  FaChartBar,
  FaFileInvoiceDollar, 
  FaClipboardList, 
  FaBox, 
  FaMoneyBillWave, 
  FaCashRegister, 
  FaCog, 
  FaSignOutAlt, 
  FaTrash, 
  FaPencilAlt,
  FaDollyFlatbed
} from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Modal, Form, Toast } from 'react-bootstrap';
import './styleFornecedor.css';
import axios from "axios";

const Fornecedor = () => {

  const [fornecedor, setFornecedor] = useState([]);
  const [itemSelecionado, setItemSelecionado] = useState(null);
  const [modalAberto, setModalAberto] = useState(false);
  const [modoEditar, setModoEditar] = useState(false);
  const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  const [itemToDelete, setItemToDelete] = useState(null);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");
  const [formValido, setFormValido] = useState(false);

  const [cnpj, setCNPJ] = useState("");
  const [nome, setNome] = useState("");

  function validarCNPJ(cnpj) {
    cnpj = cnpj.replace(/[^\d]+/g,'');
 
    if(cnpj === '') return false;
     
    if (cnpj.length !== 14) return false;
 
    // Elimina CNPJs invalidos conhecidos
    if (cnpj === "00000000000000" || 
        cnpj === "11111111111111" || 
        cnpj === "22222222222222" || 
        cnpj === "33333333333333" || 
        cnpj === "44444444444444" || 
        cnpj === "55555555555555" || 
        cnpj === "66666666666666" || 
        cnpj === "77777777777777" || 
        cnpj === "88888888888888" || 
        cnpj === "99999999999999")
        return false;
         
    // Calcula o primeiro dígito verificador
    let soma = 0;
    for (let i = 0; i < 12; i++) {
      soma += parseInt(cnpj.charAt(i)) * (i < 4 ? 5 - i : 13 - i);
    }
    let digito1 = (11 - soma % 11) % 10;
    
    // Calcula o segundo dígito verificador
    soma = 0;
    for (let i = 0; i < 13; i++) {
      soma += parseInt(cnpj.charAt(i)) * (i < 5 ? 6 - i : 14 - i);
    }
    let digito2 = (11 - soma % 11) % 10;
    
    // Verifica se os dígitos verificadores estão corretos
    if (parseInt(cnpj.charAt(12)) !== digito1 || parseInt(cnpj.charAt(13)) !== digito2) {
      return false;
    }
    
    // CNPJ válido
    return true;
  }

  function handleCNPJChange(event) {
    setCNPJ(event.target.value);
  }

  function handleNomeChange(event) {
    setNome(event.target.value);
  }

  function validarFormulario() {
    if (!validarCNPJ(cnpj)) {
      alert("CNPJ inválido!");
      return false;
    }
    return true;
  }

  function getFornecedor() {
    axios.get('https://localhost:44334/Fornecedor')
    .then(response => {
      setFornecedor(["", ...response.data]);
    })
    .catch(error => {
      console.log(error);
    });
  }

  function formatCnpj(cnpj) {
    return cnpj.replace(/\D/g, '').replace(/(\d{2})(\d)/, "$1.$2").replace(/(\d{3})(\d)/, "$1.$2").replace(/(\d{3})(\d)/, "$1/$2").replace(/(\d{4})(\d)/, "$1-$2");
  }

  const cnpjSemFormatacao = cnpj.replace(/[^\d]+/g,'');

  const handleAdicionar = (event) => {
    event.preventDefault();

    if (!validarFormulario()) return;

    const novoFornecedor = {
      cnpj: cnpj,
      nome: nome,
    };

    axios.post("https://localhost:44334/Fornecedor/", novoFornecedor)
    .then(response => {
      getFornecedor();
      setSuccessMessage("Fornecedor inserido com Sucesso!")
      setShowSuccessToast(true)
    })
    .catch(error => {
      console.log(error);
      setErrorMessage("Erro ao salvar fornecedor.")
      setShowErrorToast(true)
    });

    setCNPJ("");
    setNome("");
    setModalAberto(false);
    setFormValido(false);
  }

  const handleEditar = (event) => {
    event.preventDefault();

    console.log(fornecedor)
    const fornecedorEditado = {
      cnpj: itemSelecionado.cnpj,
      nome: nome,
    };
  
    axios.put("https://localhost:44334/Fornecedor/", fornecedorEditado)
    .then(response => {
      getFornecedor();
      setSuccessMessage("Fornecedor editado com sucesso!")
      setShowSuccessToast(true)
    })
    .catch(error => {
      console.log(error);
      setErrorMessage(error.message || "Erro ao editar fornecedor.")
      setShowErrorToast(true)
    });
  
    setCNPJ("");
    setNome("");
    setItemSelecionado(null);
    setModalAberto(false);
  }
  
  function handleCloseDeleteConfirmation(confirmed) {
    if (confirmed) {
      axios.delete(`https://localhost:44334/Fornecedor/${itemToDelete.cnpj}`)
      .then(response => {
        getFornecedor();
        setSuccessMessage("Fornecedor excluído com sucesso!")
        setShowSuccessToast(true)
      })
      .catch(error => {
        console.log(error);
        setErrorMessage(error.response.data.error || "Erro ao excluir Fornecedor.")
        setShowErrorToast(true)
      });
    }
    setShowDeleteConfirmation(false);
    setItemToDelete(null);
  }
  
  useEffect(() => {
    axios.get('https://localhost:44334/Fornecedor')
    .then(response => {
      setFornecedor([...response.data]);
    })
    .catch(error => {
      console.log(error);
    });
  }, []);

  const userToken = localStorage.getItem("user_token");

  const adicionarFornecedor = () => {
    setCNPJ("");
    setNome("");
    setModalAberto(true);
    setModoEditar(false);
  };

  const editarFornecedor = (item) => {
    setItemSelecionado(item);
    setCNPJ(item.cnpj);
    setNome(item.nome);
    setModalAberto(true);
    setModoEditar(true);
  };

  const removerFornecedor = (item) => {
    setItemToDelete(item);
    setShowDeleteConfirmation(true);
  };

  return (
    <Container fluid style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <img className="col-2 p-0" src={LogoCompre} alt="Logo" style={{borderRadius: 7, textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}/>
        <div className="col" style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}>
          <label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>FORNECEDORES</label>
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
      <Row id="barra de nav" className="justify-content-md-center">
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
        <Col className="pt-4 ">
          <Table striped hover>
            <thead>
              <tr>
                <th className="text-center">CNPJ</th>
                <th>Fornecedor</th>
                <th className="text-center" style={{ verticalAlign: "middle"}}>
                  <Button 
                    variant="warning" 
                    className="custom-button-add" 
                    style={{ 
                      height: "35px", 
                      width: "100px", 
                      marginBottom: "5px", 
                      color:"grey" 
                    }} 
                    onClick={() => adicionarFornecedor()}
                  >
                    Adicionar
                  </Button>
                </th>
              </tr>
            </thead>
            <tbody>
              {fornecedor.map((item, index) => (
                <tr key={index}>
                  <td style={{ verticalAlign: "middle", textAlign: "center"}}>{item.cnpj && formatCnpj(item.cnpj)}</td>
                  <td style={{ verticalAlign: "middle"}}>{item.nome}</td>
                  <td className="text-center" style={{ verticalAlign: "middle"}}>
                    <Button variant="outline-secondary" style={{ border: "none"}} onClick={() => editarFornecedor(item)}>
                      <FaPencilAlt />
                    </Button>
                    <Button 
                      variant="outline-secondary" 
                      style={{ border: "none"}} 
                      onClick={() => removerFornecedor(item)}
                    >
                      <FaTrash />
                    </Button>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
          <br/>
          <Modal show={modalAberto} onHide={() => setModalAberto(false)}>
            <Modal.Header closeButton>
              <Modal.Title style={{fontWeight: "bold", color: "Grey"}}>{itemSelecionado ? "Editar Fornecedor" : "Novo Fornecedor"}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <Form onSubmit={modoEditar ? handleEditar : handleAdicionar}>
                <Form.Group controlId="cnpj" style={{marginBottom: "20px"}}>
                  <Form.Label>CNPJ</Form.Label>
                  <Form.Control
                    type="text"
                    placeholder="Digite o CNPJ"
                    value={cnpj}
                    onChange={handleCNPJChange}
                    as={InputMask}
                    mask="99.999.999/9999-99"
                    maskChar={null}
                    onBlur={() => setCNPJ(cnpjSemFormatacao)}
                    required
                    isInvalid={formValido}
                  />
                </Form.Group>
                <Form.Group controlId="nome" style={{marginBottom: "20px"}}>
                  <Form.Label>Nome</Form.Label>
                  <Form.Control type="text" placeholder="Digite o nome do fornecedor" value={nome} onChange={handleNomeChange} required isInvalid={formValido}/>
                </Form.Group>
                <Modal.Footer>
                  <Button variant="success" type="submit" onClick={() => setFormValido(true)}> Salvar </Button>
                  <Button variant="secondary" onClick={() => setModalAberto(false)}> Fechar </Button>
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
                Tem certeza que deseja excluir o fornecedor "{itemToDelete.nome}"?
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
        </Col>
      </Row>
      <br/>
      <br/>
      
        
    </Container>
  );
};

export default Fornecedor;