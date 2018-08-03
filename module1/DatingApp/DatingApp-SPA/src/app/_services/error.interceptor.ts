import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

// HttpInterceptor is used to intercep errom from API this will be handled error from API globally
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                // Get all error if it is error in http response from API
                if (error instanceof HttpErrorResponse) {
                    // Checking error per status
                    if (error.status === 401) {
                        return throwError(error.statusText);
                    }
                    const applicationError = error.headers.get('Application-Error'); // can be found from API AddApplicationError
                    console.log(applicationError);
                   // Get Exception error
                    if (applicationError) {
                        return throwError(applicationError);
                    }

                    // checking of server errors like filed valdiation etc.
                    const serverError = error.error;
                    let modalSateErrors = '';
                    if (serverError && typeof serverError === 'object') {
                        for (const key in serverError) {
                            if (serverError[key]) {
                                modalSateErrors += serverError[key] + '\n';
                            }
                        }
                    }
                    return throwError(modalSateErrors || serverError || 'Server Error');

                }
            })
        );
    }
}

export const ErrorEnterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true // dont replace existing interceptor instead this will add to the array of interceptor
};
