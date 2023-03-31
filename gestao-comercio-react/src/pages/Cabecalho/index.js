import React from "react";
import { FaUser, FaChartBar, FaMapMarkedAlt, FaClipboardList, FaBox, FaMoneyBillWave, FaCashRegister, FaCog, FaSignOutAlt } from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Button, Col, Row, Container } from 'react-bootstrap';
import './styleCabecalho.css';

const Cabecalho = () => {

  const userToken = localStorage.getItem("user_token");

  return (
    <Container style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}>
          <img src={LogoCompre} alt="Logo" height="80" style={{borderRadius: 7}}/>
        </Col>
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}><label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>MAPA DE CUSTOS</label></Col>
        <Col style={{textAlign: "right", verticalAlign: "middle", alignSelf: "center"}}>
          <Row style={{ height: '50px'}}>
            <Link 
              to="/" 
              style={{
                color: 'grey',
                textDecoration: 'none',
                display: 'flex',
                alignItems: 'center',
                gap: '10px',
                justifyContent: "flex-end"
              }}
            >
              {JSON.parse(userToken).name}
              <FaUser className="me-2" />
            </Link>
          </Row>
          <Row>
            <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
              <Link 
                to="/" 
                style={{
                  color: 'grey',
                  textDecoration: 'none',
                  display: 'flex',
                  alignItems: 'center'
                }}
              >
                <FaCog  className="me-2" />
                CONFIG
              </Link>
              <Link 
                to="/" 
                style={{
                  color: 'grey',
                  textDecoration: 'none',
                  display: 'flex',
                  alignItems: 'center',
                  gap: '10px',
                  marginLeft: "40px"
                }}
              >
                Sair
                <FaSignOutAlt  className="me-2" />
              </Link>
            </div>
          </Row>
        </Col>
      </Row>
      <br/>
      <Row className="justify-content-md-center">
        <div className="d-flex justify-content-between">
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/"><FaChartBar className="me-2" />Consolidado</Link></Button>
          <Button variant="light" className="custom-button-menu-selected" style={{color: 'grey'}}><FaMapMarkedAlt className="me-2" />Mapa de Custos</Button>
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/"><FaClipboardList className="me-2" />Pedidos</Link></Button>
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/"><FaBox className="me-2" />Estoque</Link></Button>
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/"><FaMoneyBillWave className="me-2" />Precificação</Link></Button>
          <Button variant="light" className="custom-button-menu-last"><Link style={{color: 'grey'}} className="nav-link" to="/"><FaCashRegister className="me-2" />Caixa</Link></Button>
        </div>
      </Row>
    </Container>
  );
};

export default Cabecalho;
