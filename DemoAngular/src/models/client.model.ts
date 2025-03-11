export interface IClient {
  id: number;
  guid: string;
  typeId: number;
  type: string;
  isActive: boolean;
  isDeleted: boolean;
  name: string;
  address: string;
  city: string;
  region: string;
  postalCode: string;
  country: string;
  url: string;
}
