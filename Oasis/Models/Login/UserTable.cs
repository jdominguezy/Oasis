//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Oasis.Models.Login
//{
//    public class UserTable<TUser>
//        where TUser:usuarioOasis
//    {
//        private as2oasis _as2Oasis;

//        public UserTable(as2oasis database)
//        {
//            _as2Oasis = database;
//        }

//        /// <summary>
//        /// Returns the user's name given a user id
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public string GetUserName(int userId)
//        {
//            return _as2Oasis.usuario.Find(userId).username;
//        }

//        /// <summary>
//        /// Returns a User ID given a user name
//        /// </summary>
//        /// <param name="userName">The user's name</param>
//        /// <returns></returns>
//        public string GetUserId(string userName)
//        {            
//            return _as2Oasis.usuario.Where(x=>x.username==userName).First().id_usuario.ToString();
//        }

//        /// <summary>
//        /// Returns an TUser given the user's id
//        /// </summary>
//        /// <param name="userId">The user's id</param>
//        /// <returns></returns>
//        public TUser GetUserById(string userId)
//        {
//            var _userID = Convert.ToInt32(userId);
//            return (TUser)_as2Oasis.usuario.Where(x => x.id_usuario == _userID).First();
//        }

//        /// <summary>
//        /// Returns a list of TUser instances given a user name
//        /// </summary>
//        /// <param name="userName">User's name</param>
//        /// <returns></returns>
//        public List<usuarioOasis> GetUserByName(string userName)
//        {
//           return _as2Oasis.usuario.Where(x => x.username == userName).ToList();
//        }

//        public List<TUser> GetUserByEmail(string email)
//        {
//            return (List<TUser>)_as2Oasis.usuario.Where(x => x.email == email);
//        }

//        /// <summary>
//        /// Return the user's password hash
//        /// </summary>
//        /// <param name="userId">The user's id</param>
//        /// <returns></returns>
//        public string GetPasswordHash(int userId)
//        {
//            return _as2Oasis.usuario.Where(x=>x.id_usuario == userId).FirstOrDefault().password;
//        }

//        /// <summary>
//        /// Sets the user's password hash
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <param name="passwordHash"></param>
//        /// <returns></returns>
//        public int SetPasswordHash(string userId, string passwordHash)
//        {
//            var _userID = Convert.ToInt32(userId);
//            var usuario = _as2Oasis.usuario.Where(x => x.id_usuario == _userID)
//                .FirstOrDefault();
//            usuario.password = passwordHash;
//            _as2Oasis.SaveChanges();
//            return 1;
//        }

//        /// <summary>
//        /// Returns the user's security stamp
//        /// </summary>
//        /// <param name="userId"></param>
//        /// <returns></returns>
//        public string GetSecurityStamp(string userId)
//        {
//            var _userID = Convert.ToInt32(userId);
//            return _as2Oasis.usuario.Where(x => x.id_usuario == _userID)
//                .FirstOrDefault().SecurityStamp;
//        }

//        /// <summary>
//        /// Inserts a new user in the Users table
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        public int Insert(TUser user)
//        {
//            var nuevoUsuario = new usuarioOasis();
//            nuevoUsuario.Email = user.email;
//            nuevoUsuario.nombre = user.nombre;
//            nuevoUsuario.apellido = user.apellido;
//            nuevoUsuario.password = user.PasswordHash;
//            nuevoUsuario.id_departamento = user.id_departamento;
//            _as2Oasis.usuario.Add(nuevoUsuario);
//            _as2Oasis.SaveChanges();
             

//            return 1;
//        }

//        /// <summary>
//        /// Deletes a user from the Users table
//        /// </summary>
//        /// <param name="userId">The user's id</param>
//        /// <returns></returns>
//        private int Delete(string userId)
//        {
//            var _userID = Convert.ToInt32(userId);
//            var usuario = _as2Oasis.usuario.Find(_userID);
//            usuario.activo = 0;
//            _as2Oasis.SaveChanges();
            
//            return 1;
//        }

//        /// <summary>
//        /// Deletes a user from the Users table
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        public int Delete(TUser user)
//        {
//            return Delete(user.Id);
//        }

//        /// <summary>
//        /// Updates a user in the Users table
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        public int Update(TUser user)
//        {
//            var usuario = _as2Oasis.usuario.Find(user.id_usuario);
//            usuario.Email = user.email;
//            usuario.nombre = user.nombre;
//            usuario.apellido = user.apellido;
//            usuario.password = user.PasswordHash;
//            usuario.id_departamento = user.id_departamento; 
//            _as2Oasis.SaveChanges();
            
//            return 1;
//        }

//    }
//}