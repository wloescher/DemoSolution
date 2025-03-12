import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUser } from '../models/user.model';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private users: IUser[] = [
    { "id": 1, "guid": "55004793-5DD1-433F-8091-83DAB517556B", "typeId": 1, "type": "Admin", "isActive": true, "isDeleted": false, "emailAddress": "admin@demo.com", "firstName": "Admin", "middleName": "", "lastName": "Demo", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", "phoneNumber": "" },
    { "id": 2, "guid": "EFC80328-286E-4B3F-B31A-FE60B919E81B", "typeId": 2, "type": "Client", "isActive": true, "isDeleted": false, "emailAddress": "client@demo.com", "firstName": "Client", "middleName": "", "lastName": "Demo", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", "phoneNumber": "" },
    { "id": 3, "guid": "A38C01A1-21C2-45DE-BB6B-92D96EF22841", "typeId": 3, "type": "Sales", "isActive": true, "isDeleted": false, "emailAddress": "sales@demo.com", "firstName": "Sales", "middleName": "", "lastName": "Demo", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", "phoneNumber": "" },
    { "id": 4, "guid": "4753F629-95F9-4876-A194-EBB26AFF825C", "typeId": 4, "type": "Marketing", "isActive": true, "isDeleted": false, "emailAddress": "marketing@demo.com", "firstName": "Marketing", "middleName": "", "lastName": "Demo", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", "phoneNumber": "" },
    { "id": 5, "guid": "F0DC4FB5-F7C2-4DC7-BCD3-F0A071435E6B", "typeId": 5, "type": "Accounting", "isActive": true, "isDeleted": false, "emailAddress": "accounting@demo.com", "firstName": "Accounting", "middleName": "", "lastName": "Demo", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", "phoneNumber": "" },
    { "id": 6, "guid": "FCA78DF2-C97C-4B7B-B6A7-B58649A58C29", "typeId": 6, "type": "Executive", "isActive": true, "isDeleted": false, "emailAddress": "executive@demo.com", "firstName": "Executive", "middleName": "", "lastName": "Demo", "addressLine1": "", "addressLine2": "", "city": "", "region": "", "postalCode": "", "country": "", "phoneNumber": "" },
    { "id": 1004, "guid": "B7F3400D-60C9-447E-8693-7574308A3DA9", "typeId": 1, "type": "Admin", "isActive": false, "isDeleted": false, "emailAddress": "638768806905476687@demo.com", "firstName": "First-638768806905476687", "middleName": "Middle-638768806905476687", "lastName": "Last-638768806905476687", "addressLine1": "AddressLine1-638768806905476687", "addressLine2": "AddressLine1-638768806905476687", "city": "City-638768806905476687", "region": "Region-638768806905476687", "postalCode": "Zip-476687", "country": "Country-638768806905476687", "phoneNumber": "PhoneNumber-63876880" },
    { "id": 1010, "guid": "9F9116C6-EB9F-4E0B-BA5F-3E066762F2FF", "typeId": 1, "type": "Admin", "isActive": false, "isDeleted": false, "emailAddress": "638768813459365417@demo.com", "firstName": "First-638768813459365417", "middleName": "Middle-638768813459365417", "lastName": "Last-638768813459365417", "addressLine1": "AddressLine1-638768813459365417", "addressLine2": "AddressLine1-638768813459365417", "city": "City-638768813459365417", "region": "Region-638768813459365417", "postalCode": "Zip-365417", "country": "Country-638768813459365417", "phoneNumber": "PhoneNumber-63876881" },
    { "id": 1011, "guid": "ABF39755-D480-4500-9A40-9AEF0B30C4AA", "typeId": 1, "type": "Admin", "isActive": false, "isDeleted": false, "emailAddress": "638768814176761066@demo.com", "firstName": "First-638768814176761066", "middleName": "Middle-638768814176761066", "lastName": "Last-638768814176761066", "addressLine1": "AddressLine1-638768814176761066", "addressLine2": "AddressLine1-638768814176761066", "city": "City-638768814176761066", "region": "Region-638768814176761066", "postalCode": "Zip-761066", "country": "Country-638768814176761066", "phoneNumber": "PhoneNumber-63876881" },
    { "id": 1012, "guid": "33A0A53C-D6BE-4C65-8094-F2D8D0C1617F", "typeId": 1, "type": "Admin", "isActive": false, "isDeleted": false, "emailAddress": "638768815539153702@demo.com", "firstName": "First-638768815539153702", "middleName": "Middle-638768815539153702", "lastName": "Last-638768815539153702", "addressLine1": "AddressLine1-638768815539153702", "addressLine2": "AddressLine2-638768815539153702", "city": "City-638768815539153702", "region": "Region-638768815539153702", "postalCode": "Zip-153702", "country": "Country-638768815539153702", "phoneNumber": "PhoneNumber-63876881" },
    { "id": 1013, "guid": "08941EB6-68F8-4CB7-989A-AAD8AD14D1A5", "typeId": 1, "type": "Admin", "isActive": false, "isDeleted": false, "emailAddress": "638768818810887202@demo.com", "firstName": "First-638768818810887202", "middleName": "Middle-638768818810887202", "lastName": "Last-638768818810887202", "addressLine1": "AddressLine1-638768818810887202", "addressLine2": "AddressLine2-638768818810887202", "city": "City-638768818810887202", "region": "Region-638768818810887202", "postalCode": "Zip-887202", "country": "Country-638768818810887202", "phoneNumber": "PhoneNumber-63876881" },
    { "id": 1014, "guid": "187CCDB0-EE99-4C75-A0DD-B11BC1A82695", "typeId": 6, "type": "Executive", "isActive": false, "isDeleted": false, "emailAddress": "638768820557827650@demo.com", "firstName": "First-638768820557827650", "middleName": "Middle-638768820557827650", "lastName": "Last-638768820557827650", "addressLine1": "AddressLine1-638768820557827650", "addressLine2": "AddressLine2-638768820557827650", "city": "City-638768820557827650", "region": "Region-638768820557827650", "postalCode": "Zip-827650", "country": "Country-638768820557827650", "phoneNumber": "PhoneNumber-63876882" },
    { "id": 1025, "guid": "568E1E51-0290-471A-B6D0-8385FFDE7153", "typeId": 1, "type": "Admin", "isActive": false, "isDeleted": false, "emailAddress": "638768947838811087@demo.com", "firstName": "First-638768947838811087", "middleName": "Middle-638768947838811087", "lastName": "Last-638768947838811087", "addressLine1": "AddressLine1-638768947838811087", "addressLine2": "AddressLine2-638768947838811087", "city": "City-638768947838811087", "region": "Region-638768947838811087", "postalCode": "Zip-811087", "country": "Country-638768947838811087", "phoneNumber": "PhoneNumber-63876894" },
    { "id": 1028, "guid": "60DF708E-32DD-46D7-8712-3140336669A9", "typeId": 1, "type": "Admin", "isActive": false, "isDeleted": false, "emailAddress": "638769029733692736@demo.com", "firstName": "First-638769029733692736", "middleName": "Middle-638769029733692736", "lastName": "Last-638769029733692736", "addressLine1": "AddressLine1-638769029733692736", "addressLine2": "AddressLine2-638769029733692736", "city": "City-638769029733692736", "region": "Region-638769029733692736", "postalCode": "Zip-692736", "country": "Country-638769029733692736", "phoneNumber": "PhoneNumber-63876902" }
  ];

  constructor(private http: HttpClient) { }

  getUsers(): Observable<IUser[]> {
    // TODO: Get data from Web API
    //  return this.http.get<IUser[]>('/api/users');

    // HACK: Hard code data from db
    let users = this.users;
    return of(users);
  }

  getUser(id: number): Observable<IUser | undefined> {
    // TODO: Get data from Web API
    //  return this.http.get<IUser>('/api/user/{id}');

    // HACK: Hard code data from db
    let user = this.users.find(user => user.id === id);
    return of(user);
  }
}
