import { TestBed, inject } from '@angular/core/testing';

import { LoginToNavbarService } from './login-to-navbar.service';

describe('LoginToNavbarService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoginToNavbarService]
    });
  });

  it('should be created', inject([LoginToNavbarService], (service: LoginToNavbarService) => {
    expect(service).toBeTruthy();
  }));
});
