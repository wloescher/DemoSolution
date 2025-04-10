import React, { createContext, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { useCookies } from "react-cookie";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const navigate = useNavigate();
    const cookieName = 'UserToken';
    const [cookies, setCookie, removeCookie] = useCookies([cookieName]);

    const login = (userToken) => {
        setCookie(cookieName, userToken, { path: '/', maxAge: 3600 });
        navigate('/');
    };
    const logout = () => {
        removeCookie(cookieName, { path: '/' });
        window.location.href = "/logout";
    };

    const getIsAuthenticated = () => {
        const cookie = cookies[cookieName];
        return !!cookie;
    }

    return (
        <AuthContext.Provider value={{ getIsAuthenticated, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};