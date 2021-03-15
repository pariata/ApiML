using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;




namespace Ejercicio1PruebaML
{

    public class Program
    {
        class vendorID

        {
            // Variable publica para referenciar entre clases
            public string vid { get; set; }

        }

        

        public static void Main(string[] args)
        {

            Console.WriteLine("*************************************************");
            Console.WriteLine("Bienvenido al buscador de productos de ML");
            Console.WriteLine("*************************************************");
            Console.WriteLine("Por favor introduzca el ID del vendedor a buscar : ");
            string answ = "";


            do
            {
                var venid = new vendorID(); // Invoco a la variable publica
                venid.vid = Console.ReadLine(); //Asigno el valor de la entrada a la variable publica

                if (venid.vid.Length == 9) //Validacion sobre la dimension numerica del ID
                {

                    GetItems(venid);
                    Console.WriteLine("**************************************");
                    Console.WriteLine("Deseas realizar otra consulta?");
                    Console.WriteLine("**************************************");
                    answ = Console.ReadLine();


                }
                else //Si no cumple con la condicional anterior, ofrece volver a introducir otra ID
                {
                    Console.WriteLine("**************************************");
                    Console.WriteLine("Incorrecto, desea continuar?");
                    Console.WriteLine("**************************************");
                    answ = Console.ReadLine();
                }
                    if (answ == "si" ) //Pide el valor del ID nuevamente
                    {

                        Console.WriteLine("Por favor introduzca el ID del vendedor a buscar : ");
                        
                    }
                    else if (answ == "no") //Condiconal que invoca el ciere de la aplicacion
                    {


                    Console.WriteLine("**************************************");
                    Console.WriteLine("¡Gracias por usar nuestra aplicacion!");
                    Console.WriteLine("**************************************");
                    Console.WriteLine("**************************************");
                    Console.WriteLine("Hecho por: Manuel Pariata");
                    Console.WriteLine("**************************************");

                   
          
                 
                    return;

                   

                }

                }
                
            
            while (answ  != "no"); //Mientras la entrada sea diferente de "no" se repite el ciclo

            
            

        }



        private static void GetItems(vendorID vendorID)
        {
            //"/sites/MLA/search?seller_id=179571326" URL Predefinida por el enunciado del ejercicio
            //179571326 ID del enunciado



            vendorID.vid = vendorID.vid; //llamo a la variable publica

            var client = new RestClient("https://api.mercadolibre.com"); //establezco la conexion al cliente API mediante Restsharp

            RestSharp.RestRequest request = new RestRequest("/sites/MLA/search?seller_id=" + vendorID.vid , Method.GET) { RequestFormat = DataFormat.Json }; //Definicion de la url exacta de donde obtiene la informacion, el metodo empleado, y el tipo de contenido por respuesta esperado

            request.AddHeader("Content-type", "application/json"); //Tipo del cuerpo de la respuesta

            var response = client.Execute(request); //Solicitud del contenido mediante Restsharp




            if (response.Content.Length > 481) //Si el contenido de la respuesta obtenida es mayor a 481 procede a deserializarla
            {

                var jsonresponse = JsonConvert.DeserializeObject<Root>(response.Content); //Deserializacion de la respuesta obtenida

            



            for (int i = 0; i < jsonresponse.results.Count;  i++) //Ciclo for condicional, exigiendo el recorrido hasta alcanzar el limite de las respuestas por solicitud
            {

                    // Operacion de organizacion e impresion de datos en la consola
                    int num = i + 1;
                Console.WriteLine("******************** PRODUCTO#" + num + " ***********************");
                Console.WriteLine("ID Del Producto : " + jsonresponse.results[i].id);
                Console.WriteLine("Titulo del Producto : " + jsonresponse.results[i].title);
                Console.WriteLine("ID De La Categoria : " + jsonresponse.results[i].category_id);
                Console.WriteLine("Nombre de la Categoria : " + jsonresponse.results[i].domain_id);


                    //Generacion del log

                    System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "******************** PRODUCTO#" + num + " ***********************" + "\n");
                    System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "ID Del Vendedor : " + vendorID.vid + "\n");
                    System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "ID Del Producto : " + jsonresponse.results[i].id + "\n");
                    System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "Titulo del Producto : " + jsonresponse.results[i].title + "\n" );
                    System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "ID De La Categoria : " + jsonresponse.results[i].category_id + "\n");
                    System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "Nombre de la Categoria : " + jsonresponse.results[i].domain_id + "\n");
                    

                }
            //Complemento del log
                System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "*******************************" + "\n");
                System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "Fecha de la Operacion : " + DateTime.Now + "\n");
                System.IO.File.AppendAllText(@"C:\Log\MLLog.txt", "*******************************" +  "\n");

                Console.WriteLine("**************************************");
                Console.WriteLine("**************************************");
                Console.WriteLine("Se ha generado un Archivo en C:/Log/MLLog.txt"); //Notificacion de la creacion del log
                Console.WriteLine("**************************************");

            }
            else 
            {
                Console.WriteLine("*******************************************");
                Console.WriteLine("El ID del vendedor Introducido No Existe"); //Negacion al introducir un SELLERID incorrecto
                Console.WriteLine("*******************************************");
            }

         
        }

       
        //Conversion y estructura de la respuesta obtenida por Restsharp de JSON a C#, para poder filtrar los datos
        public class Ratings
        {
            public double negative { get; set; }
            public double positive { get; set; }
            public double neutral { get; set; }
        }

        public class Transactions
        {
            public int total { get; set; }
            public int canceled { get; set; }
            public string period { get; set; }
            public Ratings ratings { get; set; }
            public int completed { get; set; }
        }

        public class Sales
        {
            public string period { get; set; }
            public int completed { get; set; }
        }

        public class Metrics
        {
            public Sales sales { get; set; }
            public Claims claims { get; set; }
            public DelayedHandlingTime delayed_handling_time { get; set; }
            public Cancellations cancellations { get; set; }
        }

        public class SellerReputation
        {
            public string level_id { get; set; }
            public string power_seller_status { get; set; }
            public Transactions transactions { get; set; }
            public Metrics metrics { get; set; }
        }

        public class Seller
        {
            public int id { get; set; }
            public string nickname { get; set; }
            public string permalink { get; set; }
            public DateTime registration_date { get; set; }
            public SellerReputation seller_reputation { get; set; }
            public bool real_estate_agency { get; set; }
            public bool car_dealer { get; set; }
            public List<string> tags { get; set; }
            public object eshop { get; set; }
        }

        public class Paging
        {
            public int total { get; set; }
            public int primary_results { get; set; }
            public int offset { get; set; }
            public int limit { get; set; }
        }

        public class Claims
        {
            public double rate { get; set; }
            public int value { get; set; }
            public string period { get; set; }
        }

        public class DelayedHandlingTime
        {
            public double rate { get; set; }
            public int value { get; set; }
            public string period { get; set; }
        }

        public class Cancellations
        {
            public double rate { get; set; }
            public int value { get; set; }
            public string period { get; set; }
        }

        public class Conditions
        {
            public List<object> context_restrictions { get; set; }
            public DateTime? start_time { get; set; }
            public DateTime? end_time { get; set; }
            public bool eligible { get; set; }
        }

        public class Metadata
        {
            public string campaign_id { get; set; }
            public string promotion_id { get; set; }
            public string promotion_type { get; set; }
        }

        public class Price
        {
            public string id { get; set; }
            public string type { get; set; }
            public Conditions conditions { get; set; }
            public double amount { get; set; }
            public int? regular_amount { get; set; }
            public string currency_id { get; set; }
            public string exchange_rate_context { get; set; }
            public Metadata metadata { get; set; }
            public DateTime last_updated { get; set; }
            public List<Price> prices { get; set; }
            public Presentation presentation { get; set; }
            public List<object> payment_method_prices { get; set; }
        }

        public class Presentation
        {
            public string display_currency { get; set; }
        }

        public class Installments
        {
            public int quantity { get; set; }
            public double amount { get; set; }
            public double rate { get; set; }
            public string currency_id { get; set; }
        }

        public class Address
        {
            public string state_id { get; set; }
            public string state_name { get; set; }
            public string city_id { get; set; }
            public string city_name { get; set; }
        }

        public class Shipping
        {
            public bool free_shipping { get; set; }
            public string mode { get; set; }
            public List<string> tags { get; set; }
            public string logistic_type { get; set; }
            public bool store_pick_up { get; set; }
        }

        public class Country
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class State
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class City
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class SellerAddress
        {
            public string id { get; set; }
            public string comment { get; set; }
            public string address_line { get; set; }
            public string zip_code { get; set; }
            public Country country { get; set; }
            public State state { get; set; }
            public City city { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        public class Struct
        {
            public double number { get; set; }
            public string unit { get; set; }
        }

        public class Value
        {
            public string id { get; set; }
            public string name { get; set; }
            public Struct @struct { get; set; }
            public object source { get; set; }
            public List<PathFromRoot> path_from_root { get; set; }
            public int results { get; set; }
        }

        public class ValueStruct
        {
            public double number { get; set; }
            public string unit { get; set; }
        }

        public class Attribute
        {
            public List<Value> values { get; set; }
            public string attribute_group_id { get; set; }
            public string attribute_group_name { get; set; }
            public object source { get; set; }
            public string value_name { get; set; }
            public string name { get; set; }
            public string value_id { get; set; }
            public ValueStruct value_struct { get; set; }
            public string id { get; set; }
        }

        public class DifferentialPricing
        {
            public int id { get; set; }
        }

        public class Result
        {
            public string id { get; set; }
            public string site_id { get; set; }
            public string title { get; set; }
            public Seller seller { get; set; }
            public double price { get; set; }
            public Price prices { get; set; }
            public object sale_price { get; set; }
            public string currency_id { get; set; }
            public int available_quantity { get; set; }
            public int sold_quantity { get; set; }
            public string buying_mode { get; set; }
            public string listing_type_id { get; set; }
            public DateTime stop_time { get; set; }
            public string condition { get; set; }
            public string permalink { get; set; }
            public string thumbnail { get; set; }
            public string thumbnail_id { get; set; }
            public bool accepts_mercadopago { get; set; }
            public Installments installments { get; set; }
            public Address address { get; set; }
            public Shipping shipping { get; set; }
            public SellerAddress seller_address { get; set; }
            public List<Attribute> attributes { get; set; }
            public object original_price { get; set; }
            public string category_id { get; set; }
            public int official_store_id { get; set; }
            public string domain_id { get; set; }
            public string catalog_product_id { get; set; }
            public List<string> tags { get; set; }
            public bool catalog_listing { get; set; }
            public bool use_thumbnail_id { get; set; }
            public int order_backend { get; set; }
            public DifferentialPricing differential_pricing { get; set; }
        }

        public class Sort
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class AvailableSort
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class PathFromRoot
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Filter
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public List<Value> values { get; set; }
        }

        public class AvailableFilter
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public List<Value> values { get; set; }
        }

        public class Root
        {
            public string site_id { get; set; }
            public Seller seller { get; set; }
            public Paging paging { get; set; }
            public List<Result> results { get; set; }
            public List<object> secondary_results { get; set; }
            public List<object> related_results { get; set; }
            public Sort sort { get; set; }
            public List<AvailableSort> available_sorts { get; set; }
            public List<Filter> filters { get; set; }
            public List<AvailableFilter> available_filters { get; set; }
        }
    } //Fin de la aplicacion
}

//***************************
//Hecho por Manuel Pariata
//***************************
