import React, { createContext, useContext } from "react";
import { useCookies } from "react-cookie";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const cookieName = 'UserToken';
    const [cookies, setCookie, removeCookie] = useCookies([cookieName]);

    const login = (username, password, rememberMe) => {
        // Replace this with your actual authentication logic
        let isValidLogin = false;

        // Create dummy user token
        let dummyToken = {
            NameIdentifier: 0,
            Name: username,
            GivenName: 'test',
            Surname: username,
            Role: username,
            RememberMe: rememberMe
        }

        // Set NameIdentifier to UserId
        if (username == 'admin' && password == 'admin') {
            dummyToken.NameIdentifier = 1;
        }
        else if (username == 'developer' && password == 'developer') {
            dummyToken.NameIdentifier = 2;
        }
        else if (username == 'sales' && password == 'sales') {
            dummyToken.NameIdentifier = 3;
        }
        else if (username == 'marketing' && password == 'marketing') {
            dummyToken.NameIdentifier = 4;
        }
        else if (username == 'accouting' && password == 'accouting') {
            dummyToken.NameIdentifier = 5;
        }
        else if (username == 'executive' && password == 'executive') {
            dummyToken.NameIdentifier = 6;
        }
        else if (username == 'client' && password == 'client') {
            dummyToken.NameIdentifier = 7;
        }

        // Check for valid login and set cookie
        isValidLogin = dummyToken.NameIdentifier != 0;
        if (isValidLogin) {
            setCookie(cookieName, dummyToken, { path: '/', maxAge: 3600 });
        }

        return isValidLogin;
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