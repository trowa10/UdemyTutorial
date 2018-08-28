import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  Users: User[];

  constructor(private userService: UserService, private alerttify: AlertifyService,
  private route: ActivatedRoute) { }

  ngOnInit() {
    // this.loadUsers();

     /* accessing the data that set from the resolver*/
     this.route.data.subscribe(data => {
      /* users is the key that assigned from the routes.ts for resolve in MemberDetailComponent */
     this.Users = data['users'];
   });
  }

    // loadUsers() {
    //   this.userService.getUsers().subscribe(
    //     (users: User[]) => {
    //      this.Users = users;
    //     }, error => {
    //       this.alerttify.error(error);
    //     }
    //   );
    // }
}
