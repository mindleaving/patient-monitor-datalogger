export interface QueryParameters {
    [key: string]: string;
}
export class ApiClient
{
    hostname: string;
    port: number;
    basePath: string;

    constructor(
        hostname: string,
        port: number,
        basePath: string)
    {
        this.hostname = hostname;
        this.port = port;
        this.basePath = basePath.endsWith('/') ? basePath : basePath + '/';
    }

    async get(path: string, query?: QueryParameters): Promise<Response>
    {
        const url = this._buildUrl(path, query);
        return await this.send('GET', url);
    }

    async post(path: string, body?: unknown, query?: QueryParameters): Promise<Response>
    {
        const url = this._buildUrl(path, query);
        return await this.send('POST', url, body);
    }

    async put(path: string, body?: unknown, query?: QueryParameters): Promise<Response>
    {
        const url = this._buildUrl(path, query);
        return await this.send('PUT', url, body);
    }

    async delete(path: string, query?: QueryParameters): Promise<Response>
    {
        const url = this._buildUrl(path, query);
        return await this.send('DELETE', url);
    }

    async send(
        method: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE', 
        url: string,
        body?: unknown): Promise<Response>
    {
        const bodyString = !body || typeof(body) === 'string' 
            ? body as BodyInit | null | undefined 
            : JSON.stringify(body) as BodyInit;
        const headers: HeadersInit = {};
        headers['Content-Type'] = 'application/json';
        return await fetch(url, {
            method: method,
            headers: headers,
            body: bodyString
        });
    }

    _buildUrl(path: string, queryParams?: QueryParameters)
    {
        let host = this.hostname;
        if(this.port !== 443 && this.port !== 80) {
            host += `:${this.port}`;
        }
        const protocol = this.port === 80 ? 'http' : 'https';
        let query = '';
        if(queryParams && Object.entries(queryParams).length > 0) {
            query = '?' + Object.keys(queryParams).map(key => `${key}=${queryParams[key]}`).join('&');
        }
        return `${protocol}://${host}${this.basePath}${path}${query}`;
    }
}
export const apiClient: { instance?: ApiClient } = {};