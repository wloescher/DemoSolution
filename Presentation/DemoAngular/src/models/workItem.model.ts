export interface IWorkItem {
  id: number;
  guid: string;
  clientId: number;
  clientName: string;
  typeId: number;
  type: string;
  statusId: number;
  status: string;
  isActive: boolean;
  isDeleted: boolean;
  title: string;
  subTitle: string;
  summary: string;
  body: string;
  createdDate: string;
  createdBy: string;
  modifiedDate: string;
  modifiedBy: string;
}
