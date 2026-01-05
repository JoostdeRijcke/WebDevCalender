import { apiRequest } from './client';

export interface UserRegistration {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export async function loginUser(email: string, password: string): Promise<void> {
  await apiRequest('/Login', {
    method: 'POST',
    body: { email, password },
  });
}

export async function loginAdmin(email: string, password: string): Promise<void> {
  await apiRequest('/Login', {
    method: 'POST',
    body: { email, password },
  });
}

export async function getCurrentUser(): Promise<{ id: number }> {
  return apiRequest<{ id: number }>('/GetCurrentUser');
}

export async function isUserLoggedIn(): Promise<boolean> {
  try {
    const response = await apiRequest<{ isLoggedIn: boolean }>('/IsUserLoggedIn');
    return response.isLoggedIn;
  } catch {
    return false;
  }
}

export async function isAdminLoggedIn(): Promise<boolean> {
  try {
    const response = await apiRequest<{ isAdminLoggedIn: boolean }>('/IsAdminLoggedIn');
    return response.isAdminLoggedIn;
  } catch {
    return false;
  }
}

export async function logout(): Promise<void> {
  await apiRequest('/Logout');
}

export async function adminLogout(): Promise<void> {
  await apiRequest('/AdminLogout');
}

export async function requestPasswordReset(email: string): Promise<void> {
  await apiRequest('/generatecode', {
    method: 'POST',
    body: { email },
  });
}

export async function verifyResetCode(email: string, code: number): Promise<boolean> {
  try {
    await apiRequest('/checkcode', {
      method: 'POST',
      body: { email, code },
    });
    return true;
  } catch {
    return false;
  }
}

export async function resetPassword(email: string, password: string): Promise<void> {
  await apiRequest('/password', {
    method: 'PUT',
    body: { email, password },
  });
}

export async function registerUser(userData: UserRegistration): Promise<void> {
  await apiRequest('/Register', {
    method: 'POST',
    body: userData,
  });
}
