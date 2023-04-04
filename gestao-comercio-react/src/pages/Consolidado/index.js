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
  FaSignOutAlt
} from "react-icons/fa";
import { Link } from "react-router-dom";
import LogoCompre from "../../LogoCompre.png";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { Button, Col, Row, Container } from 'react-bootstrap';
import './styleConsolidado.css';
import axios from "axios";
import { Calendar, momentLocalizer } from 'react-big-calendar'
import moment from 'moment'
import 'moment/locale/pt-br';
import 'react-big-calendar/lib/css/react-big-calendar.css'

const Consolidado = () => {

  const [events, setEvents] = useState([]);
  const [currentDate, setCurrentDate] = useState(moment());

  const handleNavigate = (newDate, view) => {
    setCurrentDate(moment(newDate));
  };

  useEffect(() => {
    var mes = currentDate.month() + 1;
    var ano = currentDate.year();
    var anoMes = ano + mes.toString().padStart(2, "0");
    console.log(anoMes)
    axios
    .get(`https://localhost:44334/Caixa/getConsolidado?data=${anoMes}`)
    .then((response) => {
      setEvents(response.data);
    })
    .catch((error) => {
      console.log(error);
    });
  }, [currentDate]);

  const messages = {
    allDay: 'Dia inteiro',
    previous: 'Mês Anterior',
    next: 'Próximo Mês',
    today: 'Hoje',
    month: 'Mês',
    week: 'Semana',
    day: 'Dia',
    agenda: 'Agenda',
    date: 'Data',
    time: 'Hora',
    event: 'Evento',
  };

  function eventStyleGetter(event, start, end, isSelected) {
    let backgroundColor = '';
    let color = '';
    if (event.tipo === 'positivo') {
      backgroundColor = '#DAF5F0';
      color = '#007A5D';
    } else if (event.tipo === 'negativo') {
      backgroundColor = '#FBE1EA';
      color = '#FF2F15';
    } 
    const style = {
      backgroundColor,
      borderRadius: '5px',
      opacity: 0.8,
      color,
      fontWeight: 'bold',
      border: '0px',
      display: 'block',
      wordSpacing: '20px',
    };
    return {
      style,
    };
  }

  const userToken = localStorage.getItem("user_token");
  const localizer = momentLocalizer(moment)

  function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  moment.updateLocale('pt-br', {
    months: moment.localeData().months().map((m) => capitalize(m)),
    monthsShort: moment.localeData().monthsShort().map((m) => capitalize(m))
  });

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
        <Col className="pt-4">
          <Calendar
            localizer={localizer}
            events={events}
            startAccessor="start"
            endAccessor="end"
            views={['month']}
            messages={messages}
            eventPropGetter={eventStyleGetter}
            style={{ height: 700 }}
            showAllEvents={true}
            dayLayoutAlgorithm={'no-overlap'}
            onNavigate={handleNavigate}
          />
        </Col>
      </Row>
      <br/>
      <br/>
      
    </Container>
  );
};

export default Consolidado;