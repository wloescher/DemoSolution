import { Component } from '@angular/core';
import { IWorkItem } from '../../../models/workItem.model';
import { faAdd } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'demo-workitem-list',
  templateUrl: './workitem-list.component.html',
  styleUrls: ['./workitem-list.component.css']
})
export class WorkItemListComponent {
  workItems: IWorkItem[];
  faAdd = faAdd;

  constructor() {
    this.workItems = [
      {
        id: 1,
        guid: 'ebc3a841-13b7-4445-a8c1-8e5affdc37af',
        clientId: 1,
        clientName: 'Internal Client',
        typeId: 1,
        type: 'User Story',
        statusId: 1,
        status: 'New',
        isActive: true,
        isDeleted: false,
        title: 'Test WorkItem #1',
        subTitle: 'Lorem ipsum odor amet, consectetuer adipiscing elit.',
        summary: 'Lorem ipsum odor amet, consectetuer adipiscing elit. Conubia hac consectetur lobortis laoreet dictum elit. Et elit volutpat lectus; eros luctus vehicula praesent primis. Rhoncus non orci torquent ultrices suspendisse. Eget sagittis venenatis; nibh ornare platea eu. Nunc morbi hendrerit dui praesent torquent aenean. Scelerisque lacinia mauris cubilia finibus arcu lobortis cubilia. Imperdiet sagittis arcu nunc pellentesque proin est ipsum.',
        body: ''
      },
      {
        id: 1,
        guid: 'ebc3a841-13b7-4445-a8c1-8e5affdc37af',
        clientId: 1,
        clientName: 'Internal Client',
        typeId: 2,
        type: 'Task',
        statusId: 2,
        status: 'In Planning',
        isActive: true,
        isDeleted: false,
        title: 'Test WorkItem #3',
        subTitle: 'Lorem ipsum odor amet, consectetuer adipiscing elit.',
        summary: 'Lorem ipsum odor amet, consectetuer adipiscing elit. Auctor fringilla aenean consectetur iaculis primis phasellus. Platea ultricies accumsan ante commodo netus fermentum lacus. Vivamus ridiculus vulputate primis nec integer primis dictum maecenas rhoncus? Interdum ligula parturient facilisi potenti; mus magna! Penatibus bibendum sem curae torquent interdum metus. Magnis est magnis eget sollicitudin lacus; ac ligula senectus.',
        body: ''

      },
      {
        id: 1,
        guid: 'ebc3a841-13b7-4445-a8c1-8e5affdc37af',
        clientId: 1,
        clientName: 'Internal Client',
        typeId: 3,
        type: 'Bug',
        statusId: 3,
        status: 'In Progress',
        isActive: true,
        isDeleted: false,
        title: 'Test WorkItem #3',
        subTitle: 'Lorem ipsum odor amet, consectetuer adipiscing elit.',
        summary: 'Lorem ipsum odor amet, consectetuer adipiscing elit. Nullam purus aliquam laoreet eros nec lacus turpis feugiat. Justo primis suscipit penatibus integer ad varius senectus natoque phasellus. At class nullam placerat hendrerit lacus condimentum. Faucibus porttitor etiam; habitasse odio at dignissim! Viverra sit curae vivamus volutpat quisque elit maximus et. Sagittis class litora molestie tincidunt scelerisque lacinia mauris scelerisque. Vulputate sit hac ultrices id platea habitant magnis purus.',
        body: ''

      }
    ];
  }

  addWorkItem() {
    alert('TODO: Create WorkItem');
  }
}
