import { useEffect, useState } from "react";
import loadingStatus from "../helpers/loadingStatus";

const useClients = () => {
    const [clients, setClients] = useState([]);
    const [loadingState, setLoadingState] = useState(loadingStatus.isLoading);

    useEffect(() => {
        const fetchClients = async () => {
            setLoadingState(loadingStatus.isLoading);
            try {
                //const response = await fetch('/test/client?includeInactive=true');
                //const clients = await response.json();

                const clients = [
                    { "id": 1, "guid": "F01A5647-CEC8-4531-A376-32386244E142", "typeId": 1, "type": "Internal", "isActive": true, "isDeleted": false, "name": "Internal Client", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", phoneNumber: "phoneNumber", "url": "https:\/\/internal.demo.com" },
                    { "id": 2, "guid": "85374721-A546-4CA7-B303-8B9A5BF0E7AA", "typeId": 2, "type": "External", "isActive": true, "isDeleted": false, "name": "External Client", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", phoneNumber: "phoneNumber", "url": "https:\/\/external.demo.com" },
                    { "id": 3, "guid": "5B499196-7BDC-48D6-BE13-2F633C5D6E9F", "typeId": 3, "type": "Lead", "isActive": true, "isDeleted": false, "name": "Lead Client", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", phoneNumber: "phoneNumber", "url": "https:\/\/lead.demo.com" },
                    { "id": 8, "guid": "607AFB07-F38B-4185-837C-A24EF77ED217", "typeId": 1, "type": "Internal", "isActive": false, "isDeleted": false, "name": "Name-638769029731528592", "addressLine1": "Address-638769029731528592", "addressLine2": "Address-638769029731528592", "city": "City-638769029731528592", "region": "Region-638769029731528592", "postalCode": "Zip-528592", "country": "Country-638769029731528592", phoneNumber: "phoneNumber-63876902", "url": "Url-638769029731528592" },
                    { "id": 11, "guid": "A51987DF-B1EF-4D9E-BFD5-E288A4FC5E6F", "typeId": 1, "type": "Internal", "isActive": false, "isDeleted": false, "name": "Name-638769091915310588", "addressLine1": "Address-638769091915310588", "addressLine2": "Address-638769091915310588", "city": "City-638769091915310588", "region": "Region-638769091915310588", "postalCode": "Zip-310588", "country": "Country-638769091915310588", phoneNumber: "phoneNumber-63876909", "url": "Url-638769091915310588" }
                ];

                setClients(clients);
                setLoadingState(loadingStatus.loaded);
            } catch {
                setLoadingState(loadingStatus.hasErrored);
            }
        };
        fetchClients();
    }, []);

    return { clients, setClients, loadingState };
};

export default useClients;