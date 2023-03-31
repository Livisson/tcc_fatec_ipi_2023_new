import React, { useState, useEffect } from "react";
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import { FaUser, FaChartBar, FaMapMarkedAlt, FaClipboardList, FaBox, FaMoneyBillWave, FaCashRegister, FaCog, FaSignOutAlt} from "react-icons/fa";
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

  /*const events = [
    {
      title: 'Receita: R$5000,00',
      start: new Date(2023, 2, 27, 9, 0),
      end: new Date(2023, 2, 27, 10, 0),
      tipo: 'positivo'
    },
    {
      title: 'Despesa: R$3000,00',
      start: new Date(2023, 2, 27, 10, 0),
      end: new Date(2023, 2, 27, 11, 0),
      tipo: 'negativo'
    },
    {
      title: 'Resumo: R$3000,00',
      start: new Date(2023, 2, 27, 11, 0),
      end: new Date(2023, 2, 27, 12, 0),
      tipo: 'positivo'
    },
    {
      title: 'Receita: R$4000,00',
      start: new Date(2023, 2, 28, 9, 0),
      end: new Date(2023, 2, 28, 10, 0),
      tipo: 'positivo'
    },
    {
      title: 'Despesa: R$6000,00',
      start: new Date(2023, 2, 28, 10, 0),
      end: new Date(2023, 2, 28, 11, 0),
      tipo: 'negativo'
    },
    {
      title: 'Despesa: -R$2000,00',
      start: new Date(2023, 2, 28, 11, 0),
      end: new Date(2023, 2, 28, 12, 0),
      tipo: 'negativo'
    },
  ];*/

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
    <Container style={{ backgroundColor: "white" }}>
      <Row className="justify-content-md-center">
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}}>
          <img src={LogoCompre} alt="Logo" height="80" style={{borderRadius: 7}}/>
        </Col>
        <Col style={{textAlign: "left", verticalAlign: "middle", alignSelf: "center"}} xs={6}><label style={{fontSize:22, fontWeight: "bold", color: "gray"}}>CONSOLIDADO: RECEITAS E DESPESAS</label></Col>
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
          <Button variant="light" className="custom-button-menu-selected"><Link style={{color: 'grey'}} className="nav-link" to="/consolidado"><FaChartBar className="me-2" />Consolidado</Link></Button>
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
          <Button variant="light" className="custom-button-menu"><Link style={{color: 'grey'}} className="nav-link" to="/precificar"><FaMoneyBillWave className="me-2" />Precificação</Link></Button>
          <Button variant="light" className="custom-button-menu-last"><Link style={{color: 'grey'}} className="nav-link" to="/caixa"><FaCashRegister className="me-2" />Caixa</Link></Button>
        </div>
      </Row>
      <br/>
      <br/>
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
    </Container>
  );
};

export default Consolidado;
