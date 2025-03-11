import { Component } from '@angular/core';
import { IWorkItem } from '../../../models/workItem.model';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'demo-workitem-detail',
  templateUrl: './workitem-detail.component.html',
  styleUrls: ['./workitem-detail.component.css']
})
export class WorkItemDetailComponent {
  workItem: IWorkItem;
  faTrash = faTrash;

  constructor() {
    this.workItem = {
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
      body: `Lorem ipsum odor amet, consectetuer adipiscing elit. Hendrerit adipiscing consequat rhoncus netus volutpat mollis maecenas. Est aliquet fusce nisi rutrum quis massa platea pretium. Ex massa tristique fermentum feugiat primis gravida donec felis. Mauris malesuada non nam; adipiscing leo quis sociosqu. Imperdiet netus erat dictum rhoncus blandit aptent malesuada leo. Nostra nam magna habitant odio ut lacinia netus.

Eu sodales potenti fringilla blandit senectus finibus ipsum.Vivamus felis ipsum tempus neque iaculis magna nostra scelerisque nisl.Eleifend lacinia euismod maecenas platea magna libero venenatis blandit.Mattis penatibus porttitor mattis pellentesque efficitur.Taciti lacinia id facilisi proin feugiat fusce sollicitudin dis.Turpis velit sapien et cursus finibus.Ut sit blandit leo cursus curabitur.Amet montes eu semper, dui ultricies pellentesque nibh egestas lacus.Nec neque non volutpat curae ligula nam torquent.

Ipsum torquent pharetra et vitae venenatis nullam conubia metus.Quam sit suspendisse scelerisque inceptos accumsan praesent.Morbi magna sem interdum enim neque varius quisque curae.Morbi congue sed suspendisse, posuere lectus facilisi torquent.Viverra facilisis porttitor aliquam facilisis, primis rhoncus magnis.Nulla ligula fringilla proin sit dictum nascetur mauris imperdiet placerat.Hendrerit posuere efficitur tempor blandit dui risus mollis.

Amet laoreet risus pretium auctor lorem habitasse massa justo.Integer sollicitudin velit lobortis volutpat natoque magnis curabitur dis.Hendrerit pharetra orci vestibulum dictumst, maximus sociosqu diam.Justo platea enim habitasse blandit at urna eget scelerisque eros.Habitant hendrerit nibh adipiscing convallis tortor platea? Feugiat elementum nascetur penatibus turpis ad.Avehicula proin torquent ex suspendisse.Aliquet vivamus penatibus eros et; eros quisque commodo morbi tristique.Dolor dictum justo ultrices dignissim taciti maecenas scelerisque.Dapibus orci facilisi hendrerit hac lectus sollicitudin facilisi etiam.

Proin velit phasellus consectetur ultricies scelerisque posuere ac tortor.Ami facilisi platea venenatis sociosqu nullam, tortor vestibulum.Odio rhoncus mauris etiam montes velit gravida at.Quis netus auctor viverra accumsan dui senectus.Accumsan lacinia maecenas magna; pretium accumsan tristique.Augue suscipit dolor tempor non vitae.Fermentum nostra parturient sociosqu, rhoncus gravida nulla porta.

Primis facilisis condimentum tortor ante neque venenatis ex.Dapibus sociosqu risus dictum ante consectetur tincidunt.Odio odio ultrices integer taciti risus.In malesuada ullamcorper eu tempus ullamcorper justo laoreet nascetur.Platea efficitur ornare aenean platea viverra ut.Amet pulvinar pulvinar lectus auctor finibus; quisque tincidunt risus.Taciti consequat consequat semper aliquam; fermentum integer nec.Ornare proin orci scelerisque amet imperdiet senectus habitant.Quisque semper dignissim porta malesuada venenatis mus amet imperdiet diam.Efficitur blandit velit facilisi libero nunc dapibus.

Massa erat vestibulum sapien dis eros sociosqu maecenas.Dignissim laoreet nibh habitant lacus litora.Lacinia sollicitudin urna nisl tempor litora; nunc volutpat.Erat velit conubia proin; eleifend lobortis nascetur.Eu aliquet a aliquam imperdiet aenean, fringilla etiam? Commodo tristique maecenas fringilla sollicitudin ullamcorper vulputate consectetur.Nec pellentesque nullam diam non tempor platea leo nullam dignissim.Scelerisque scelerisque molestie placerat dolor phasellus class.

Vulputate vestibulum mus nec hendrerit, nulla curae platea dignissim.Elit montes mi mus diam dictumst facilisi scelerisque.Ornare varius sed faucibus posuere ac consectetur congue.Maecenas vivamus class morbi sodales amet tincidunt venenatis.Malesuada odio ullamcorper et aenean posuere diam arcu purus.Torquent pulvinar arcu phasellus facilisi, nunc ligula per turpis.Mattis nec dolor aliquet congue turpis penatibus egestas.Sollicitudin amet tellus; lectus habitasse adipiscing nec.Pulvinar ac aptent scelerisque augue commodo vel.Pulvinar nostra quis tempor ligula efficitur adipiscing penatibus penatibus.`,
    }
  }

  deleteWorkItem() {
    console.log('TODO: Delete WorkItem');
  }
}
