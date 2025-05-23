import { lazy, Suspense } from 'react'
import { Routes, Route, Navigate, Outlet } from 'react-router';
import Spinner from 'react-bootstrap/Spinner';
import Login from './Components/Login';
import Logout from './Components/Logout';
import AccessDenied from './Components/AccessDenied';
import RouteNotFound from './Components/RouteNotFound';
import PrivateRoutes from "./components/PrivateRoutes";
import { AuthProvider } from "./utils/AuthProvider";

import NavBar from './components/NavBar'
import Error from './components/Error';
import './App.css'

const Home = lazy(() => import('./App/Home/Home'));
const ClientList = lazy(() => import('./App/Client/ClientList'));
const ClientDetail = lazy(() => import('./App/Client/ClientDetail'));
const ClientEdit = lazy(() => import('./App/Client/ClientEdit'));

const UserList = lazy(() => import('./App/User/UserList'));
const UserDetail = lazy(() => import('./App/User/UserDetail'));
const UserEdit = lazy(() => import('./App/User/UserEdit'));

const WorkItemList = lazy(() => import('./App/WorkItem/WorkItemList'));
const WorkItemDetail = lazy(() => import('./App/WorkItem/WorkItemDetail'));
const WorkItemEdit = lazy(() => import('./App/WorkItem/WorkItemEdit'));

//const Search = lazy(() => import('./App/Search'));

function App() {
    return (
        <AuthProvider>
            <NavBar />
            <Suspense fallback={<div className="container text-secondary"><Spinner size="sm" animation="border" role="status" /> Loading...</div>}>
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/logout" element={<Logout />} />
                    <Route element={<PrivateRoutes />}>
                        <Route path="/accessdenied" element={<AccessDenied />} />

                        <Route path="/" element={<Home />} />
                        <Route path="/clients" element={<ClientList />} />
                        <Route path="/clients/:filter" element={<ClientList />} />
                        <Route path="/client/:id" element={<ClientDetail />} />
                        <Route path="/client/:id/edit" element={<ClientEdit />} />
                        <Route path="/client/add" element={<ClientEdit />} />

                        <Route path="/users" element={<UserList />} />
                        <Route path="/users/:filter" element={<UserList />} />
                        <Route path="/user/:id" element={<UserDetail />} />
                        <Route path="/user/:id/edit" element={<UserEdit />} />
                        <Route path="/user/add" element={<UserEdit />} />

                        <Route path="/workitems" element={<WorkItemList />} />
                        <Route path="/workitems/:filter" element={<WorkItemList />} />
                        <Route path="/workitem/:id" element={<WorkItemDetail />} />
                        <Route path="/workitem/:id/edit" element={<WorkItemEdit />} />
                        <Route path="/workitem/add" element={<WorkItemEdit />} />

                        {/*<Route path="/search" element={<Search />} />*/}
                    </Route>
                    <Route path="*" element={<RouteNotFound />} />
                </Routes>
            </Suspense>
        </AuthProvider>
    )
}

export default App
