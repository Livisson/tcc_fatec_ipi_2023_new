import React, { useState, useEffect } from "react";
import Input from "../../components/Input";
import Button from "../../components/Button";
import * as C from "./styles";
import { Link, useNavigate } from "react-router-dom";
import useAuth from "../../hooks/useAuth";
import './style.css';
import { FaUser } from 'react-icons/fa';
import LogoCompre from "../../LogoCompre.png"
import backgroundImage from '../../LogoDAT3.jpg';
import axios from "axios";
import { Toast } from 'react-bootstrap';

const Signin = () => {

  const { signin, signout } = useAuth();
  const navigate = useNavigate();

  

  useEffect(() => {
    console.log("Entrou")
    console.log(localStorage.getItem("selectedWindow"))
    console.log(localStorage.getItem("selectedWindow") === "home");
    
    if (localStorage.getItem("selectedWindow") === "home") {
      signout();
    }

    window.addEventListener('popstate', () => {
      localStorage.setItem("selectedWindow", "home");
      //signout()
    });
    return () => {
      window.removeEventListener('popstate', () => {
      localStorage.setItem("selectedWindow", "home");
        //signout()
    })};

    
  }, []);

  const [nome, setNome] = useState("");
  const [senha, setSenha] = useState("");
  const [error, setError] = useState("");

  const [showErrorToast, setShowErrorToast] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [showSuccessToast, setShowSuccessToast] = useState(false);
  const [successMessage, setSuccessMessage] = useState("");

  const handleLogin = () => {
    if (!nome | !senha) {
      setError("Preencha todos os campos");
      return;
    }

    //const res = signin(nome, senha);

    signin(nome, senha)
      .then((res) => {
        console.log(res)
        if (res) {
          setError(res);
          return;
        }
        navigate("/consolidado");
      })
      .catch((error) => {
        console.log(error.toString())
        setError(error.toString().replace('Error: ', ''));
      });

    /*console.log(res)
    if (res) {
      setError(res);
      return;
    }

    navigate("/despesas");*/

  };
  
  function recuperarSenha() {
    axios
      .get("https://localhost:44334/api/Usuario/getRecuperarSenha")
      .then((response) => {
        setSuccessMessage("E-mail enviado com sucesso!")
        setShowSuccessToast(true)
      })
      .catch((error) => {
        console.log(error);
        setErrorMessage(error.response.data.error || "Erro ao enviar o e-mail.")
        setShowErrorToast(true)
      });
  }
  
  const backgroundStyle = {
    backgroundImage: `url(${backgroundImage})`,
    backgroundRepeat: 'no-repeat',
    //backgroundSize: 'cover',
    //backgroundPosition: 'center',
    minHeight: '100vh', // opcional: define a altura mínima da tela inteira
  };

  return (
    <C.ContentCapa style={backgroundStyle}>
      <C.Container>
        <C.ContentImg>
          <img src={LogoCompre} alt="Logo" />
        </C.ContentImg>
        <C.Content>
          <C.Label>
            LOGIN  
            <FaUser size={16} />
          </C.Label>
          <Input
            type="text"
            placeholder="Digite seu Nome"
            value={nome}
            onChange={(e) => [setNome(e.target.value), setError("")]}
          />
          <Input
            type="password"
            placeholder="Digite sua Senha"
            value={senha}
            onChange={(e) => [setSenha(e.target.value), setError("")]}
          />
          <C.labelError>{error}</C.labelError>
          <Button Text="Entrar" onClick={handleLogin} />
          <C.LabelSignup>
            <C.Strong>
              <Link onClick={recuperarSenha}>Esqueceu a senha? Clique aqui!</Link>
            </C.Strong>
          </C.LabelSignup>
          <Toast show={showErrorToast} onClose={() => setShowErrorToast(false)} bg="danger" delay={3000} autohide>
            <Toast.Body className="text-white">{errorMessage}</Toast.Body>
          </Toast>
          <Toast show={showSuccessToast} onClose={() => setShowSuccessToast(false)} bg="success" delay={3000} autohide>
            <Toast.Body className="text-white">{successMessage}</Toast.Body>
          </Toast> 
        </C.Content>
      </C.Container> 
    </C.ContentCapa>
    
  );
};

export default Signin;