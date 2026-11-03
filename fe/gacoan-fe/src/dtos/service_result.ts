export interface ServiceResult<T> {
    success: boolean;
    data: T | undefined;
    message: string | undefined;
    statusCode: number;
}