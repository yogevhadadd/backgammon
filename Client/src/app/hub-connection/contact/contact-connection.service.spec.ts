import { TestBed } from '@angular/core/testing';

import { ContactConnectionService} from './contact-connection.service';

describe('HomeConnectionService', () => {
  let service: ContactConnectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContactConnectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
