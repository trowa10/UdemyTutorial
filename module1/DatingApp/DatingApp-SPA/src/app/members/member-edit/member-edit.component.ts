import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '../../../../node_modules/@angular/router';
import { User } from '../../_models/user';
import { AlertifyService } from '../../_services/alertify.service';
import { NgForm } from '../../../../node_modules/@angular/forms';
import { UserService } from '../../_services/user.service';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  user: User;

  // Hostlistener Bind CSS event
  // example prevent the user by leaving the page
  // Note: message that will display are deafult for browser.
  @HostListener('window:beforeunload', ['$event'])

  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
              private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.user = data['user'];
      }
    );
  }

  UpdateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user)
        .subscribe(next => {
          this.editForm.reset(this.user);
          this.alertify.success('Profile successfully edit');
        }, error => {
          this.alertify.error(error);
        });

  }

}
