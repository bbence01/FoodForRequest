import { Component, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { trigger, state, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  animations: [
    trigger('scrollAnimation', [
      state('show', style({ transform: 'translateY(0)' })),
      state('hide', style({ transform: 'translateY(-100%)' })),
      transition('show <=> hide', animate('300ms ease-in-out')),
    ]),
  ],
})
export class NavbarComponent {
  constructor(public authService: AuthService, private router: Router) {}

  scrollState = 'show';
  lastScrollPosition = 0;


  @HostListener('window:scroll', [])
  onWindowScroll() {
    const currentScrollPosition = window.pageYOffset;

    if (currentScrollPosition > this.lastScrollPosition) {
      this.scrollState = 'hide';
    } else {
      this.scrollState = 'show';
    }

    this.lastScrollPosition = currentScrollPosition;
  }

  logout() {
    this.authService.doLogout();
    this.router.navigateByUrl('/login');
  }

  redirectIfNotLoggedIn() {
    if (!this.authService.isLoggedIn) {
      this.router.navigateByUrl('/login');
    }
  }
}
