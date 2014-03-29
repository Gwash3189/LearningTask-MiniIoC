using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniIocAdam
{
	class MainClass
	{
		internal interface IUser{
			string Name {
				get;
				set;
			}
		}
		internal interface IStudent : IUser{

			int StudentNumber {
				get;
				set;
			}

		}

		internal class User: IUser{
			public string Name {
				get;
				set;
			}
		}

		internal class Studnet: IStudent{

			public Studnet ()
			{
				
			}

			public Studnet (string name, int studentNumber)
			{
				Name = name;
				StudentNumber = studentNumber;
			}

			public string Name {
				get;
				set;
			}

			public int StudentNumber {
				get;
				set;
			}
		}


		public static void Main (string[] args)
		{
			var c = new IoCContainer ();
			var Id = "AnotherUser";
			c.Register<IUser,User>();
			c.Register<IUser,User>(() => {return new User{Name = Id};},Id);
			c.Register<IStudent, Studnet> ();

			Console.WriteLine (c.Resolve<IUser> ());
			Console.WriteLine (c.Resolve<IUser> (Id).Name);
			Console.WriteLine (c.Resolve<IStudent> ());

		}
	}

	interface IIoCContainer {
		//TFromType = Type that will be Registered (ISomethingClass)
		//ToType = Concrete Implementation that will really be returned (SomethingClass: ISomethingClass)


		void Register (Type ToType, Type FromType, string Id);
		//where word defines that TToType must implement TFromType
		//http://msdn.microsoft.com/en-us/library/bb384067.aspx
		void Register<TFromType,TToType> ( string Id = "") where TToType: TFromType;

		void Register<TFromType, TToType> (Func<object> concreteFactory, string Id = "")where TToType: TFromType;

		void Register(Type FromType ,Type ToType,Func<object> concreteFactory, string Id = "");

		bool IsRegistered (string Id);

		bool IsRegistered (Type ToType, string Id);

		bool IsRegistered<TFromType, TToType> ();

		bool IsRegistered<TFromType, TToType> (string Id);

		object Resolve (Type ToType, string Id = "");

		TFromType Resolve<TFromType> ();

		TFromType Resolve<TFromType> (string id);

	}

	public class IoCContainer : IIoCContainer{
		public readonly List<IocRegister> RegisteredObjects = new List<IocRegister>();


		public void Register <TFromType,TToType>(string Id = "") where TToType : TFromType{
			Register<TFromType, TToType> (() => Activator.CreateInstance (typeof(TToType)), Id);
		}

		public void Register(Type ToType, Type FromType ,string Id = ""){
			Register (ToType, FromType,() => Activator.CreateInstance (ToType), Id);
		}

		public void Register<TFromType, TToType> (Func<object> concreteFactory, string Id = "") where TToType : TFromType{
			Register (typeof(TFromType), typeof(TToType), concreteFactory, Id);
		}


		public void Register(Type FromType, Type ToType, Func<object> concreteFactory, string Id = ""){
			if (RegisteredObjects.FindAll(x => x.ToType == ToType).ToList().Count() == 0 
				|| RegisteredObjects.FindAll(x => x.Id == Id).ToList().Count == 0) {
				RegisteredObjects.Add (new IocRegister {
					FromType = FromType,
					ToType = ToType,
					Id = Id,
					factory = concreteFactory
				});
				return;
			}
			throw new Exception ("Item Already Exsits");
		}

		public bool IsRegistered(Type ToType, string Id){
			return RegisteredObjects.Exists (x => x.ToType == ToType && x.Id == Id);
		}

		public bool IsRegistered(string Id){
			return RegisteredObjects.Exists (x => x.Id == Id);
		}

		public bool IsRegistered<TFromType,TToType>(){
			return RegisteredObjects.Exists (x => x.FromType == typeof(TFromType) && x.ToType == typeof(TToType));
		}

		public bool IsRegistered<TFromType,TToType>(string Id){
			return RegisteredObjects.Exists (x => x.FromType == typeof(TFromType) && x.ToType == typeof(TToType) && x.Id == Id);
		}

		public TFromType Resolve<TFromType>(){
			var item = RegisteredObjects.Find (x => x.FromType == typeof(TFromType));
			var newItem = item.factory ();
			return (TFromType) newItem;
		}

		public TFromType Resolve<TFromType>(string Id= ""){
			if (Id == "") {
				return (TFromType)RegisteredObjects.Find (x => x.FromType == typeof(TFromType) && x.Id == Id).factory ();
			}

			var item =  (TFromType)RegisteredObjects.Find (x => x.FromType == typeof(TFromType) && x.Id == Id).factory ();
			return item;
		}

		public object Resolve (Type ToType, string Id = ""){
			return RegisteredObjects.Find (x => x.ToType == ToType && x.Id == Id).factory ();
		}
	}

	public class IocRegister {
		public Type FromType {
			get;
			set;
		}
		public Type ToType {
			get;
			set;
		}
		public string Id {
			get;
			set;
		}
		public Func<object> factory{ 
			get; 
			set;
		}
	}
}
