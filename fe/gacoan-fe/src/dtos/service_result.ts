export interface ServiceResult<T>
{
    Success: boolean;
    Data: T | undefined;
    Message: string | undefined;
    StatusCode: number;
}