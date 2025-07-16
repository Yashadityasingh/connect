import { TestBed } from '@angular/core/testing';
import { HttpRequest, HttpHandler } from '@angular/common/http';

import { JwtInterceptor } from './jwt-interceptor';
import { AuthService } from '../services/auth';

describe('JwtInterceptor', () => {
  let interceptor: JwtInterceptor;
  let httpHandlerSpy: jasmine.SpyObj<HttpHandler>;

  beforeEach(() => {
    // We won't spy on AuthService; interceptor will read token directly
    TestBed.configureTestingModule({
      providers: [JwtInterceptor]
    });

    interceptor = TestBed.inject(JwtInterceptor);
    httpHandlerSpy = jasmine.createSpyObj('HttpHandler', ['handle']);

    // Pre-load a token into localStorage for the test:
    localStorage.setItem('token', 'fake-token');
  });

  afterEach(() => {
    localStorage.removeItem('token');
  });

  it('should add Authorization header when token exists', () => {
    const req = new HttpRequest('GET', '/test');
    interceptor.intercept(req, httpHandlerSpy);
    const handledReq = httpHandlerSpy.handle.calls.mostRecent().args[0] as HttpRequest<any>;
    expect(handledReq.headers.has('Authorization')).toBeTrue();
    expect(handledReq.headers.get('Authorization')).toBe('Bearer fake-token');
  });
});
