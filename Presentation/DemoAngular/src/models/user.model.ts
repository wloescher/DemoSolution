export interface IUser {
  id: number;
  guid: string;
  typeId: number;
  type: string;
  isActive: boolean;
  isDeleted: boolean;
  emailAddress: string;
  firstName: string;
  middleName: string;
  lastName: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  region: string;
  postalCode: string;
  country: string;
  phoneNumber: string;
}
