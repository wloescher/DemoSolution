import { lazy, Suspense } from 'react'
import { Routes, Route } from 'react-router-dom';
import RouteNotFound from './Components/RouteNotFound';
import NavBar from './components/NavBar'
import Error from './components/Error';
import './App.css'

const Home = lazy(() => import('./App/Home/home'));
//const Search = lazy(() => import('./App/Search'));
//const Client = lazy(() => import('./App/Client'));
//const ClientEdit = lazy(() => import('./App/Client/Edit'));

function App() {
    return (
        <>
            <NavBar />
            <Suspense fallback={<div className="container">Loading...</div>}>
                <Routes>
                    <Route path="/" element={<Home />} />
                    {/*<Route path="/search" element={<Search />} />*/}
                    {/*<Route path="/client/:clientId" element={<Client />} />*/}
                    {/*<Route path="/client/:clientId/edit" element={<ClientEdit />} />*/}
                    {/*<Route path="/contract/:contractId" element={<Contract />} />*/}
                    {/*<Route path="/content/:contentId" element={<Content />} />*/}
                    <Route path="*" element={<RouteNotFound />} />
                </Routes>
            </Suspense>
        </>
    )
}

export default App
