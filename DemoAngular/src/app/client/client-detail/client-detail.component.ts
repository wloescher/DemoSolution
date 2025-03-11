import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { IClient } from '../../../models/client.model';
import { ClientService } from '../../../services/client.service';

@Component({
  selector: 'demo-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.css']
})
export class ClientDetailComponent {
  client: any;
  id: number = 0;
  faTrash = faTrash;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private clientSvc: ClientService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.id = parseInt(params.get('id') ?? '0') ?? 0;
    });
    this.clientSvc.getClient(this.id).subscribe((client: IClient | undefined) => this.client = client);
  }

  deleteClient() {
    console.log('TODO: Delete Client');
  }
}
