import { lazy, Suspense } from 'react'
import { Routes, Route } from 'react-router-dom';
import RouteNotFound from './Components/RouteNotFound';
import NavBar from './components/NavBar'
import Error from './components/Error';
import './App.css'

const Home = lazy(() => import('./App/Home/Home'));
const ClientDetail = lazy(() => import('./App/Client/ClientDetail'));
const UserDetail = lazy(() => import('./App/User/UserDetail'));
const WorkItemDetail = lazy(() => import('./App/WorkItem/WorkItemDetail'));
//const Search = lazy(() => import('./App/Search'));
//const ClientEdit = lazy(() => import('./App/Client/Edit'));

function App() {
    return (
        <>
            <NavBar />
            <Suspense fallback={<div className="container">Loading...</div>}>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/client/:id" element={<ClientDetail />} />
                    <Route path="/user/:id" element={<UserDetail />} />
                    <Route path="/workitem/:id" element={<WorkItemDetail />} />
                    {/*<Route path="/search" element={<Search />} />*/}
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
