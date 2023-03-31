import { Fragment } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import Home from "../pages/Home";
import Consolidado from "../pages/Consolidado";
import Despesas from "../pages/Despesas";
import Pedido from "../pages/Pedido";
import Fornecedor from "../pages/Fornecedor";
import Produto from "../pages/Produto";
import Estoque from "../pages/Estoque";
import Precificar from "../pages/Precificar";
import Caixa from "../pages/Caixa";
import Config from "../pages/Config";

const Private = ({ Item }) => {
  const { signed } = useAuth();

  return signed > 0 ? <Item /> : <Home />;
};

const RoutesApp = () => {
  return (
    <BrowserRouter>
      <Fragment>
        <Routes>
          <Route exact path="/config" element={<Private Item={Config} />} />
          <Route exact path="/caixa" element={<Private Item={Caixa} />} />
          <Route exact path="/precificar" element={<Private Item={Precificar} />} />
          <Route exact path="/estoque" element={<Private Item={Estoque} />} />
          <Route exact path="/pedidos" element={<Private Item={Pedido} />} />
          <Route exact path="/fornecedores" element={<Private Item={Fornecedor} />} />
          <Route exact path="/produtos" element={<Private Item={Produto} />} />
          <Route exact path="/despesas" element={<Private Item={Despesas} />} />
          <Route exact path="/consolidado" element={<Private Item={Consolidado} />} />
          <Route path="/" element={<Home />} />
          <Route path="*" element={<Home />} />
        </Routes>
      </Fragment>
    </BrowserRouter>
  );
};

export default RoutesApp;