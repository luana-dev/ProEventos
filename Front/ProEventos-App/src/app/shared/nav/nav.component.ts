import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '@app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  isCollapsed = true;

  constructor(private route: Router,
              public accountService: AccountService) { }

  ngOnInit(): void {
  }

  logout(): void{
    this.accountService.logout();
    this.route.navigateByUrl('/user/login');
  }

  showMenu(): boolean{
    return this.route.url !== '/user/login';
  }
}
