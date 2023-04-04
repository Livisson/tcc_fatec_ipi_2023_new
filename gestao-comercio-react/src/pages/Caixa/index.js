import React, { useState, useEffect } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import InputMask from 'react-input-mask';
import { 
  FaUser, 
  FaFileContract, 
  FaCog, 
  FaTrash,
  FaSignOutAlt, 
  FaBarcode
} from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Table, Button, Col, Row, Container, Form, Modal, InputGroup } from 'react-bootstrap';
import './styleCaixa.css';
import axios from "axios";

const Caixa = () => {

  
  const [itemSelecionado, setItemSelecionado] = useState(null);
  const [caixaAberto, setCaixaAberto] = useState(false);
  const [showCancelarConfirmation, setShowCancelarConfirmation] = useState(false);
  const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  const [itemToDelete, setItemToDelete] = useState(null);
  const [produtoPesquisado, setProdutoPesquisado] = useState(null);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");

  const [barras, setBarras] = useState("");
  const [valor, setValor] = useState("");
  const [quantidade, setQuantidade] = useState("");
  const [produtos, setProdutos] = useState([]);
  
  function handleBarrasChange(event) {
    setBarras(event.target.value);
  }

  useEffect(() => {
    if (!barras) {
      return;
    }
    axios
      .get(`https://localhost:44334/Caixa?request=${barras}`)
      .then((response) => {
        setProdutoPesquisado(response.data.nome);
        setValor(response.data.valorVenda);
      })
      .catch((error) => {
        console.log(error);
        setProdutoPesquisado(null);
        setValor("");
      });
  }, [barras]);

  const handleAdicionar = () => {
    console.log(barras);
    
    axios
    .get(`https://localhost:44334/Caixa?request=${barras}`)
    .then((response) => {
      console.log("Entrou");
      console.log(response);
      const novoProduto = {
        nome: response.data.nome,
        codigoBarras: response.data.codigoBarras,
        valor: Number(response.data.valorVenda),
      };

      setProdutos([...produtos, novoProduto]);
      setSuccessMessage("Produto adicionado com sucesso.");
      setShowSuccessToast(true);
    })
    .catch((error) => {
      console.log(error);
      setErrorMessage("Erro ao pesquisar produto.");
      setShowErrorToast(true);
    });
    
    setProdutoPesquisado(null);
    setValor("");
    setBarras("");
  };

  const removerProduto = (produto) => {
    setItemToDelete(produto);
    setShowDeleteConfirmation(true);
  };

  function handleDeletarProduto(confirmed) {
    if (confirmed) {
      const novosProdutos = produtos.filter(p => p !== itemToDelete);
      setProdutos(novosProdutos);
    }
    setShowDeleteConfirmation(false);
    setItemToDelete(null);
  }

  function handleCancelarCompra(confirmed) {
    if (confirmed) {
      setProdutos([]);
    }
    setShowCancelarConfirmation(false);
  }

  const cancelarCompra = () => {
    setShowCancelarConfirmation(true);
  };
  
  useEffect(() => {
    console.log("Entrou");
    axios.get('https://localhost:44334/Caixa')
    .then(response => {
      setProdutos([...response.data]);
    })
    .catch(error => {
      console.log(error);
    });
  }, []);

  const Total = ({ produtos }) => {
    const total = produtos.reduce((total, produto) => total + produto.valor, 0);
    return (
      <div className="total">
        <span>Total: R${total.toFixed(2)}</span>
      </div>
    );
  };

  const userToken = localStorage.getItem("user_token");

  return (
    <Container fluid style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <img className="col-2 p-0" src={LogoCompre} alt="Logo" style={{borderRadius: 7, textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}/>
        <div className="col" style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}>
          <label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>CAIXA</label>
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
                  <Link to="/consolidado" style={{color: 'grey', textDecoration: 'none', display: 'flex', alignItems: 'center'}}>
                    <FaFileContract className="me-2" />Administrativo
                  </Link>
                </Dropdown.Item>
                
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
      <br/>
      <br/>
      <Row>
        <Col>
          <Row>
            <Table striped hover>
              <thead>
                <tr>
                  <th className="text-center">ITEM</th>
                  <th className="text-center">EAN</th>
                  <th className="text-center">DESCRIÇÃO</th>
                  <th className="text-center">QUANT.</th>
                  <th className="text-center">UNIT.</th>
                  <th className="text-center">DESC. %</th>
                  <th className="text-center">R$ FINAL</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {produtos.map((produto, index) => (
                  <tr key={index}>
                    <td>{index}</td>
                    <td>{produto.codigoBarras}</td>
                    <td>{produto.nome}</td>
                    <td></td>
                    <td>R$ {produto.valor},00</td>
                    <th></th>
                    <th></th>
                    <th>
                      <Button 
                      variant="outline-secondary" 
                      style={{ border: "none"}} 
                      onClick={() => removerProduto(produto)}>
                        <FaTrash style={{color: 'red'}}/>
                      </Button>
                    </th>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Row>
          <Total produtos={produtos} />
        </Col>
        <Col xs={3} style={{ verticalAlign: "middle", textAlign: "center"}}>
          <Form.Label>Códigos de Barras</Form.Label>
          
          <InputGroup id="barras" className="mb-3">
            <InputGroup.Text id="basic-addon1"><FaBarcode/></InputGroup.Text>
            <Form.Control
              as={InputMask}
              mask="9999999999999"
              alwaysShowMask
              value={barras}
              onChange={handleBarrasChange}
            >
            </Form.Control>
          </InputGroup>
          <Form.Label>Nome do Produto</Form.Label>
          <InputGroup id="nome" className="mb-3">
            <Form.Control
              type="text"
              aria-label="Nome do produto"
              aria-describedby="basic-addon1"
              value={produtoPesquisado ?? ""}
              readOnly
            />
          </InputGroup>
          <Row>
            <Col>
              <Form.Label>Quantidade</Form.Label>
              <InputGroup className="mb-3">
                <Form.Control placeholder=""/>
              </InputGroup>
            </Col>
            <Col>
              <Form.Label>Valor Unitario</Form.Label>
              {/* <InputGroup id="valor" className="mb-3">
                <Form.Control placeholder=""/>
              </InputGroup> */}
              <InputGroup className="mb-3">
                <InputGroup.Text id="valor">R$</InputGroup.Text>
                <Form.Control
                  type="text"
                  aria-label="Valor do produto"
                  aria-describedby="valor"
                  value={valor ?? ""}
                  readOnly
                />
              </InputGroup>
            </Col>
          </Row>
          <Row className="mt-4 mb-3 ps-3 pe-3">
            <Button size="sm" onClick={handleAdicionar}>
              CONFIRMAR ITEM
            </Button>
          </Row>
          <Row className="mt-4 mb-3 ps-3 pe-3">
            <Button variant="danger" size="sm" onClick={cancelarCompra}>
              CANCELAR COMPRA
            </Button>
          </Row>
          <Row className="mt-4 mb-3 ps-3 pe-3">
            <Button variant="success" size="sm">
              FINALIZAR COMPRA
            </Button>
          </Row>
        </Col>
      </Row>
      {showDeleteConfirmation && (
        <Modal show={showDeleteConfirmation} onHide={() => handleDeletarProduto(false)}>
          <Modal.Header closeButton>
            <Modal.Title>Confirmação de exclusão</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Tem certeza que deseja retirar o produto "{itemToDelete.nome}" do caixa?
          </Modal.Body>
          <Modal.Footer>
            <Button variant="danger" onClick={() => handleDeletarProduto(true)}>
              Confirmar
            </Button>
            <Button variant="secondary" onClick={() => handleDeletarProduto(false)}>
              Cancelar
            </Button>
          </Modal.Footer>
        </Modal>
      )}
      {showCancelarConfirmation && (
        <Modal show={showCancelarConfirmation} onHide={() => handleCancelarCompra(false)}>
          <Modal.Header closeButton>
            <Modal.Title>Confirmação de cancelamento</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Tem certeza que deseja cancelar a compra?
          </Modal.Body>
          <Modal.Footer>
            <Button variant="danger" onClick={() => handleCancelarCompra(true)}>
              Confirmar
            </Button>
            <Button variant="secondary" onClick={() => handleCancelarCompra(false)}>
              Cancelar
            </Button>
          </Modal.Footer>
        </Modal>
      )}
    </Container>
  );
};

export default Caixa;