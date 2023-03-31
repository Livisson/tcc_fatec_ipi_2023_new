import { createContext, useEffect, useState } from "react";
import axios from "axios";

export const AuthContext = createContext({});

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState();

  useEffect(() => {
    const userToken = localStorage.getItem("user_token");
    const usersStorage = localStorage.getItem("users_bd");

    console.log(userToken)
    console.log(usersStorage)
    if (userToken && usersStorage) {
      const hasUser = JSON.parse(usersStorage)?.filter(
        (user) => user.name === JSON.parse(userToken).name
      );

      if (hasUser) setUser(hasUser[0]);
    }
  }, []);

  const signin = (name, password) => {

    const jobject = {
      Username: name,
      Password: password,
    };

    /*
    axios
      .post("https://localhost:44334/api/Token/Authenticate", jobject)
      .then((response) => {
        console.log(response);
        if (response.status === 200) {
          const usersStorage = JSON.parse(localStorage.getItem("users_bd"));

          let newUser;
          if (usersStorage) {
            newUser = [...usersStorage, { name, password }];
          } else {
            newUser = [{ name, password }];
          }
          localStorage.setItem("users_bd", JSON.stringify(newUser));

          const token = response.data;
          localStorage.setItem(
            "user_token",
            JSON.stringify({ name, token })
          );
          setUser({ name, password });
          return;
        }
        else {
          return "Nome ou senha incorretos";
        }
      })
      .catch((erro) => {
        //console.log(erro);
        return Promise.reject("Nome ou senha incorretos");
      });*/
      //return "Nome ou senha incorretos";

      return axios.post("https://localhost:44334/api/Token/Authenticate", jobject)
      .then((response) => {
        if (response.status === 200) {
          const usersStorage = JSON.parse(localStorage.getItem("users_bd"));
  
          let newUser;
          if (usersStorage) {
            newUser = [...usersStorage, { name, password }];
          } else {
            newUser = [{ name, password }];
          }
          localStorage.setItem("users_bd", JSON.stringify(newUser));
  
          const token = response.data;
          localStorage.setItem("user_token", JSON.stringify({ name, token }));
  
          setUser({ name, password });
        }
        else {
          throw new Error("Nome ou senha incorretos");
        }
      })
      .catch((erro) => {
        throw new Error("Nome ou senha incorretos");
      });
  };

  const signout = () => {
    setUser(null);
    localStorage.removeItem("user_token");
    localStorage.removeItem("users_bd");
  };

  return (
    <AuthContext.Provider
      value={{ user, signed: !!user, signin, signout }}
    >
      {children}
    </AuthContext.Provider>
  );
};