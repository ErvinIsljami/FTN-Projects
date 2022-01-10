import { TestBed, inject } from '@angular/core/testing';

import { RidesService } from './rides.service';

describe('RidesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RidesService]
    });
  });

  it('should be created', inject([RidesService], (service: RidesService) => {
    expect(service).toBeTruthy();
  }));
});
