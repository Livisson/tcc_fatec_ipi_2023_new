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
import { Table, Button, Col, Row, Container, Modal, Form, InputGroup, Toast } from 'react-bootstrap';
import './styleCaixa.css';
import axios from "axios";

const Caixa = () => {

  const [showCancelarConfirmation, setShowCancelarConfirmation] = useState(false);
  const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
  const [itemToDelete, setItemToDelete] = useState(null);
  const [produtoPesquisado, setProdutoPesquisado] = useState(null);
  const [showFinalizarConfirmation, setShowFinalizarConfirmation] = useState(false);

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");

  const [barras, setBarras] = useState("");
  const [valor, setValor] = useState("");
  const [quantidade, setQuantidade] = useState(1);
  const [totalProdutoSelecionado, setTotalProdutoSelecionado] = useState(0);
  const [produtos, setProdutos] = useState([]);
  
  function handleBarrasChange(event) {
    setBarras(event.target.value);
  }

  useEffect(() => {
    if (!barras) return;
    
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
        produto: {
          nome: response.data.nome,
          codigoBarras: response.data.codigoBarras,
          valor: Number(response.data.valorVenda),
        },
        quantidade: quantidade,
      };
      
      const produtoExistente = produtos.find(
        (p) => p.produto.codigoBarras === novoProduto.produto.codigoBarras
      );
      if (produtoExistente) {
        produtoExistente.quantidade += quantidade;
        produtoExistente.total += Number(response.data.valorVenda) * quantidade;
        setTotalProdutoSelecionado(produtoExistente.total);
        setProdutos([...produtos]);
      } 
      else {
        const novoTotal = Number(response.data.valorVenda) * quantidade;
        setTotalProdutoSelecionado(novoTotal);
        setProdutos([...produtos, { produto: novoProduto.produto, quantidade, total: novoTotal }]);
      }

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
    setQuantidade(1);
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

  const cancelarCompra = () => {
    setShowCancelarConfirmation(true);
  };

  function handleCancelarCompra(confirmed) {
    if (confirmed) setProdutos([]);
    setShowCancelarConfirmation(false);
  }
  
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
    const total = produtos.reduce((total, produto) => total + produto.produto.valor * produto.quantidade, 0);
    return (
      <div className="total">
        <span>Total: R$ {total.toFixed(2)}</span>
      </div>
    );
  };

  const finalizarCaixa = (produto) => {
    setShowFinalizarConfirmation(true);
  };

  const handleFinalizarCaixa = () => {
  
    for (let i = 0; i < produtos.length; i++) {
      const produto = produtos[i];
      console.log("Finalizando");
      
      const novoEnvio = {
        codigoBarras: produto.produto.codigoBarras,
        nomeProduto: produto.produto.nome,
        quantidade: parseInt(produto.quantidade),
      };

      console.log(novoEnvio);
      
      axios.post("https://localhost:44334/Caixa/", novoEnvio)
      .then(response => {
        console.log("Enviado");
        console.log(response);
      })
      .catch(error => {
        console.log("Não Enviado");
        console.log(error);
      });
    }
  
    setProdutos([]);
    setShowSuccessToast(true);
    setSuccessMessage("Caixa finalizado com sucesso!");
    setShowFinalizarConfirmation(false);
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
      <Row className="pt-4 ">
        <Col className="ps-4 pe-3" xs={9} style={{ verticalAlign: "middle", textAlign: "center"}}>
          <Row>
            <Table striped hover>
              <thead>
                <tr>
                  <th className="text-center">ITEM</th>
                  <th className="text-center">EAN</th>
                  <th className="text-center">DESCRIÇÃO</th>
                  <th className="text-center">QUANT.</th>
                  <th className="text-center">UNIT.</th>
                  <th className="text-center">R$ FINAL</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {produtos.map((produto, index) => (
                  <tr key={index}>
                    <td>{index}</td>
                    <td>{produto.produto.codigoBarras}</td>
                    <td style={{ textAlign: "left"}}>{produto.produto.nome}</td>
                    <td>{produto.quantidade}</td>
                    <td>R$ {produto.produto.valor.toFixed(2)}</td>
                    <th>R$ {produto.total.toFixed(2)}</th>
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
              value={barras}
              as={InputMask}
              mask="9999999999999"
              maskChar={null}
              onChange={handleBarrasChange}
            />
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
              <InputGroup id="qnt" className="mb-3">
                <Form.Control
                  type="number"
                  min="1"
                  id="qnt"
                  value={quantidade}
                  onChange={(event) => setQuantidade(parseInt(event.target.value))}
                />
              </InputGroup>
            </Col>
            <Col>
              <Form.Label>Valor Unitario</Form.Label>
              {/* <InputGroup id="valor" className="mb-3">
                <Form.Control placeholder=""/>
              </InputGroup> */}
              <InputGroup className="mb-3">
                <InputGroup.Text id="valor">R$ </InputGroup.Text>
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
            <Button variant="success" size="sm" onClick={finalizarCaixa}>
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
            Tem certeza que deseja retirar o produto "{itemToDelete.produto.nome}" do caixa?
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
      {showFinalizarConfirmation && (
        <Modal show={showFinalizarConfirmation} onHide={() => handleFinalizarCaixa(false)}>
          <Modal.Header closeButton>
            <Modal.Title>Confirmação de Finalização</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Tem certeza que deseja finalizar a compra?
          </Modal.Body>
          <Modal.Footer>
            <Button variant="danger" onClick={() => handleFinalizarCaixa(true)}>
              Confirmar
            </Button>
            <Button variant="secondary" onClick={() => handleFinalizarCaixa(false)}>
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

export default Caixa;