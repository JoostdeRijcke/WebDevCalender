export const BASE_URL = 'http://localhost:5001/api';

export type RequestOptions = {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
  body?: any;
  headers?: Record<string, string>;
};

export async function apiRequest<T>(
  endpoint: string,
  options?: RequestOptions
): Promise<T> {
  const { method = 'GET', body, headers = {} } = options || {};

  const config: RequestInit = {
    method,
    headers: {
      'Content-Type': 'application/json',
      ...headers,
    },
    credentials: 'include', // Always include credentials for session cookies
  };

  if (body) {
    config.body = JSON.stringify(body);
  }

  const url = endpoint.startsWith('http') ? endpoint : `${BASE_URL}${endpoint}`;

  try {
    const response = await fetch(url, config);

    // Handle non-OK responses
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || `HTTP error! status: ${response.status}`);
    }

    // Check if response has content
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
      return await response.json();
    }

    // For non-JSON responses (like empty 204s)
    return null as T;
  } catch (error) {
    console.error('API Request failed:', error);
    throw error;
  }
}
