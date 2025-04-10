import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { CookiesProvider } from 'react-cookie';
import { BrowserRouter } from 'react-router-dom';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { fab } from '@fortawesome/free-brands-svg-icons';

import './index.css';
import App from './App.jsx';

library.add(fas)
library.add(fab)

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <CookiesProvider defaultSetOptions={{ path: '/', sameSite: 'strict' }}>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </CookiesProvider>
    </StrictMode>,
)
