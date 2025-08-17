import { Component } from '@angular/core';
import { RouterOutlet, RouterLinkActive , RouterLink} from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  template: `
    <app-navbar></app-navbar>
    <main class="page-container">
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [`
    .page-container {
      max-width: 900px;
      margin: 80px auto 2rem; 
      padding: 0 1rem;
    }
  `]
})
export class AppComponent {
    constructor() {

    }
}