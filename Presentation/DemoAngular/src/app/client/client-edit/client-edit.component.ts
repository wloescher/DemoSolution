import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { faX } from '@fortawesome/free-solid-svg-icons';
import { faSave } from '@fortawesome/free-solid-svg-icons';
import { IClient } from '../../../models/client.model';
import { ClientService } from '../../../services/client.service';

declare var bootbox: any;

@Component({
  selector: 'demo-client-edit',
  templateUrl: './client-edit.component.html',
  styleUrls: ['./client-edit.component.css'],
})
export class ClientEditComponent {
  id: number = 0;
  client: any;
  faTrash = faTrash;
  faX = faX;
  faSave = faSave;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private clientSvc: ClientService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.id = parseInt(params.get('id') ?? '0') ?? 0;
    });

    if (this.id === 0) {
      // Create new client
      this.client = {
        id: 0,
        guid: '',
        typeId: 0,
        type: '',
        isActive: true,
        isDeleted: false,
        name: '',
        addressLine1: '',
        addressLine2: '',
        city: '',
        region: '',
        postalCode: '',
        country: '',
        phoneNumber: '',
        url: '',
      }
    }
    else {
      // Get client
      this.clientSvc.getClient(this.id).subscribe((client: IClient | undefined) => this.client = client);
    }
  }

  saveClient() {
    this.client = this.clientSvc.saveClient(this.client);
  }

  deleteClient() {
    bootbox.confirm('Are you sure you want to delete this Client?', (result: boolean) => {
      if (result) {
        bootbox.alert('TODO: Delete Client');
      }
    });
  }
}
