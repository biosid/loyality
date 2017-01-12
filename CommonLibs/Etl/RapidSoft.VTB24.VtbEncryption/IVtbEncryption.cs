namespace RapidSoft.VTB24.VtbEncryption
{
    public interface IVtbEncryption
    {
        /// <summary>
        /// Call filepro.exe -e for encription
        /// </summary>        
        /// <param name="workspace">Working directory</param>
        void Encrypt(string workspace);

        /// <summary>
        /// Call filepro.exe -d for decription
        /// </summary>
        /// <param name="workspace">Working directory</param>
        void Decrypt(string workspace);
    }
}
