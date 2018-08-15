import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './member-list/member-list.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {
        // protecs all rout with single Guard
        path: '', // if not empty route will be example path: 'dummy' it become localhost:4200/dummymembers   same with the other.
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
            {path: 'messages', component: MessagesComponent},
            {path: 'lists', component: ListComponent}
        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'}, // all uknown page will be redirect to Home
];
