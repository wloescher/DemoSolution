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

  clientTypes = [
    { value: '0', label: 'Select...' },
    { value: '1', label: 'Internal' },
    { value: '2', label: 'External' },
    { value: '3', label: 'Lead' },
  ];

  regions = [
    { value: 'AL', label: 'Alabama[ngValue]=' },
    { value: 'AB', label: 'Alberta[ngValue]=' },
    { value: 'BC', label: 'British Columbia[ngValue]=' },
    { value: 'MB', label: 'Manitoba[ngValue]=' },
    { value: 'NB', label: 'New Brunswick[ngValue]=' },
    { value: 'NL', label: 'Newfoundland and Labrador[ngValue]=' },
    { value: 'NS', label: 'Nova Scotia[ngValue]=' },
    { value: 'NT', label: 'Northwest Territories[ngValue]=' },
    { value: 'NU', label: 'Nunavut[ngValue]=' },
    { value: 'ON', label: 'Ontario[ngValue]=' },
    { value: 'PE', label: 'Prince Edward Island[ngValue]=' },
    { value: 'QC', label: 'Quebec[ngValue]=' },
    { value: 'SK', label: 'Saskatchewan[ngValue]=' },
    { value: 'YT', label: 'Yukon[ngValue]=' },
    { value: 'ENG', label: 'England[ngValue]=' },
    { value: 'NIR', label: 'Northern Ireland[ngValue]=' },
    { value: 'SCT', label: 'Scotland[ngValue]=' },
    { value: 'WLS', label: 'Wales[ngValue]=' },
    { value: 'IOM', label: 'Isle of Man[ngValue]=' },
    { value: 'GIB', label: 'Gibraltar[ngValue]=' },
    { value: 'GGY', label: 'Guernsey[ngValue]=' },
    { value: 'JEY', label: 'Jersey[ngValue]=' },
    { value: 'AK', label: 'Alaska[ngValue]=' },
    { value: 'AZ', label: 'Arizona[ngValue]=' },
    { value: 'AR', label: 'Arkansas[ngValue]=' },
    { value: 'CA', label: 'California[ngValue]=' },
    { value: 'CO', label: 'Colorado[ngValue]=' },
    { value: 'CT', label: 'Connecticut[ngValue]=' },
    { value: 'DE', label: 'Delaware[ngValue]=' },
    { value: 'FL', label: 'Florida[ngValue]=' },
    { value: 'GA', label: 'Georgia[ngValue]=' },
    { value: 'HI', label: 'Hawaii[ngValue]=' },
    { value: 'ID', label: 'Idaho[ngValue]=' },
    { value: 'IL', label: 'Illinois[ngValue]=' },
    { value: 'IN', label: 'Indiana[ngValue]=' },
    { value: 'IA', label: 'Iowa[ngValue]=' },
    { value: 'KS', label: 'Kansas[ngValue]=' },
    { value: 'KY', label: 'Kentucky[ngValue]=' },
    { value: 'LA', label: 'Louisiana[ngValue]=' },
    { value: 'ME', label: 'Maine[ngValue]=' },
    { value: 'MD', label: 'Maryland[ngValue]=' },
    { value: 'MA', label: 'Massachusetts[ngValue]=' },
    { value: 'MI', label: 'Michigan[ngValue]=' },
    { value: 'MN', label: 'Minnesota[ngValue]=' },
    { value: 'MS', label: 'Mississippi[ngValue]=' },
    { value: 'MO', label: 'Missouri[ngValue]=' },
    { value: 'MT', label: 'Montana[ngValue]=' },
    { value: 'NE', label: 'Nebraska[ngValue]=' },
    { value: 'NV', label: 'Nevada[ngValue]=' },
    { value: 'NH', label: 'New Hampshire[ngValue]=' },
    { value: 'NJ', label: 'New Jersey[ngValue]=' },
    { value: 'NM', label: 'New Mexico[ngValue]=' },
    { value: 'NY', label: 'New York[ngValue]=' },
    { value: 'NC', label: 'North Carolina[ngValue]=' },
    { value: 'ND', label: 'North Dakota[ngValue]=' },
    { value: 'OH', label: 'Ohio[ngValue]=' },
    { value: 'OK', label: 'Oklahoma[ngValue]=' },
    { value: 'OR', label: 'Oregon[ngValue]=' },
    { value: 'PA', label: 'Pennsylvania[ngValue]=' },
    { value: 'RI', label: 'Rhode Island[ngValue]=' },
    { value: 'SC', label: 'South Carolina[ngValue]=' },
    { value: 'SD', label: 'South Dakota[ngValue]=' },
    { value: 'TN', label: 'Tennessee[ngValue]=' },
    { value: 'TX', label: 'Texas[ngValue]=' },
    { value: 'UT', label: 'Utah[ngValue]=' },
    { value: 'VT', label: 'Vermont[ngValue]=' },
    { value: 'VA', label: 'Virginia[ngValue]=' },
    { value: 'WA', label: 'Washington[ngValue]=' },
    { value: 'WV', label: 'West Virginia[ngValue]=' },
    { value: 'WI', label: 'Wisconsin[ngValue]=' },
    { value: 'WY', label: 'Wyoming[ngValue]=' },
  ];

  countries = [
    { value: '', label: 'Select...' },
    { value: 'Canada', label: 'Canada' },
    { value: 'UK', label: 'United Kingdom' },
    { value: 'USA', label: 'United States of America' },
  ];

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
      };
    } else {
      // Get client
      this.clientSvc
        .getClient(this.id)
        .subscribe((client: IClient | undefined) => (this.client = client));

    }
  }

  saveClient() {
    this.client = this.clientSvc.saveClient(this.client);
  }

  deleteClient() {
    bootbox.confirm(
      'Are you sure you want to delete this Client?',
      (result: boolean) => {
        if (result) {
          bootbox.alert('TODO: Delete Client');
        }
      }
    );
  }
}
