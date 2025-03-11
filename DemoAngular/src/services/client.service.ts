import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IClient } from '../models/client.model';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private http: HttpClient) { }

  getClients(): Observable<IClient[]> {
    // TODO: Get data from Web API
    //  return this.http.get<IClient[]>('/api/clients');

    // HACK: Hard code data from db
    let clients = [
      { "id": 1, "guid": "F01A5647-CEC8-4531-A376-32386244E142", "typeId": 1, "type": "Internal", "isActive": true, "isDeleted": false, "name": "Internal Client", "address": "", "city": "", "region": "", "postalCode": "", "country": "", "url": "https:\/\/internal.demo.com" },
      { "id": 2, "guid": "85374721-A546-4CA7-B303-8B9A5BF0E7AA", "typeId": 2, "type": "External", "isActive": true, "isDeleted": false, "name": "External Client", "address": "", "city": "", "region": "", "postalCode": "", "country": "", "url": "https:\/\/external.demo.com" },
      { "id": 3, "guid": "5B499196-7BDC-48D6-BE13-2F633C5D6E9F", "typeId": 3, "type": "Lead", "isActive": true, "isDeleted": false, "name": "Lead Client", "address": "", "city": "", "region": "", "postalCode": "", "country": "", "url": "https:\/\/lead.demo.com" },
      { "id": 8, "guid": "607AFB07-F38B-4185-837C-A24EF77ED217", "typeId": 1, "type": "Internal", "isActive": false, "isDeleted": false, "name": "Name-638769029731528592", "address": "Address-638769029731528592", "city": "City-638769029731528592", "region": "Region-638769029731528592", "postalCode": "Zip-528592", "country": "Country-638769029731528592", "url": "Url-638769029731528592" },
      { "id": 11, "guid": "A51987DF-B1EF-4D9E-BFD5-E288A4FC5E6F", "typeId": 1, "type": "Internal", "isActive": false, "isDeleted": false, "name": "Name-638769091915310588", "address": "Address-638769091915310588", "city": "City-638769091915310588", "region": "Region-638769091915310588", "postalCode": "Zip-310588", "country": "Country-638769091915310588", "url": "Url-638769091915310588" }
    ];

    return of(clients);
  }
}
