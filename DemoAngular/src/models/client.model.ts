export interface IClient {
  id: number;
  guid: string;
  typeId: number;
  type: string;
  isActive: boolean;
  isDeleted: boolean;
  name: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  region: string;
  postalCode: string;
  country: string;
  phoneNumber: string;
  url: string;
}
