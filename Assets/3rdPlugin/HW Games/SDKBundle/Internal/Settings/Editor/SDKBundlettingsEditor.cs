using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using HWGames.Bundles.Internal.Analytics.Editor;
using UnityEditor.SceneManagement;

namespace HWGames.Bundles.Internal.Editor {
    [CustomEditor(typeof(SDKBundleSettings))]
    public class SDKBundlettingsEditor : UnityEditor.Editor {
        private const string EditorPrefEditorIDFA = "EditorIDFA";
        private const string _PackageNameLabel = "Package Name";
        public const string DeepLinkingActivityName = "com.facebook.unity.FBUnityDeepLinkingActivity";
        private static readonly string GradleTemplatePath = Path.Combine("Assets/Plugins/Android", "mainTemplate.gradle");

        private SDKBundleSettings sdkBundleSettings => target as SDKBundleSettings;
        private Texture2D LogoTexture;
        private GUIStyle _Style_FBNativeAndroidAppSettings;
        private GUIStyle environmentValueStyle;
        private GUIContent _ClassNameLabel = new GUIContent("Class Name", "aka: the activity name");
        private GUIContent _EditorAnalyticsID = new GUIContent("Analytics Id(Editor Only) ", "aka: the Analytics Id");
        private string Logo = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAYEBQYFBAYGBQYHBwYIChAKCgkJChQODwwQFxQYGBcUFhYaHSUfGhsjHBYWICwgIyYnKSopGR8tMC0oMCUoKSj/2wBDAQcHBwoIChMKChMoGhYaKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCj/wAARCAF6AXgDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD6pooooAKKKKACiiigAooooAKKKKACiiigAooooATtSH7tZ+o6naaem67uI4hgkB3wTj0HeuN1L4hwoxXTrZ5TyA8h2D2IXqfodv8AhtRw1Wt8ETlrYulR+NnoRPy8mqN3qNnZhftd3DBu6eY4XP0zXkGo+LdWvmbN00Cbg2yD5McY6jn8Cf6VjSyPIzvIxZ3JJJOSSepJr1aOS1JfxHY8utncI6U1c9auvHejQRFopZbh842RxkH/AMewP1rHuPiRGJGFtprsn8LPMEP/AHzg4rzqjivQhk1CPxXZ5884xEvhaR2P/CwtX/542f8A3w/+NUB4y15mx9uP/flP8K52uo8A6L/aep+dMn+jW+0nPRm7VdXDYbDU3NwRlSxGJxFRQUmd94Xj1U2Ym1e6eSRxuEflqu0fgM5roh9KFGBQK+TnLmbdj6+lDkja5yfjD+2La1N5pd4ypGMyQ+WjfL6jjNcH/wAJnr3T7ef+/Kf4V7I6CSNkYAhhgivF/F2jNpGrSoA32eT54z7f3fwr18pdGrelVirniZrCtT/e0ptLqaq/EPV/+eFj/wB8P/jWlb/EdDIBc6ayp/EyShz+WBmvPaK9eWVYaX2TyYZpiYfaPWrTx1o00ZMsstuemySMk/8AjuRXQ2epWd5v+yXcM+zGfLcPj8q8EoikaNkeNiroQQQcEEdCDXFUyOH2JWO2lnlRfGrn0PkEetH05rxPTPFmr2DLtu2nTcW2T/Pn8Tz/AMBB/rXWaX8Q4HIXUbd4jkAvEd49yV6j6Dd/j5lbKq9LVK/oepRzXD1dG7M9Bo/Cs7TdTs9QTfZ3EcowCQj9M+o7Voj6157i4uzPSjJSV4sdRRRQUFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQA3FLVS9vILO1knuZVjiQZLt2rznX/AB5cSO8WkARxKcCdhlm+gPA/HPHoa6KGFqYh2gjlr4ynh1ebO21vXLHSYibucCTqI05c9cce+MZPFcDrPju/uX26cotIAeGwHc9fvZ4GR2wee9ci8jyM8kjl3cliSckk9STTK+iw2T0qetTVnzmJzerU0hoiSaWWeRpZ5Hlc9Xckk9uSaZSUtetCMY6I8mUubWQUUUVZIUUUUASW0MlzOkMIYu5CgDuTXt3hvS00jS47ZMbsZc+p71xnwx0XdLJqdwnA+SHI/M16TXymbYv2k/Zx2R9TlGE5Ie2lux1FFFeOe4NxyK53xpo39r6SyIF+0R/PGff0/GujpOMGqp1HTmpx6GVWkqsHCXU+d2UozBgwYfKQaK6/4i6J9h1D7bCMQXB5wOA//wBeuQr7fC11iKamj4fEYd0KjgwooorqOcKKKKAHQTy28glgkeJx0dCQR24IrrtF8eX9s+3UFF3CTy+Ajjp93HB47YHPeuOpa46+Do1/jR1UcVWw/wADPbdD12y1ePNpOpk6mN+HHTPHtnGRxW32r55hkeNkeJmR0IYEHBBHRlNdt4f8eXMbLFq6iWJjgzqMFevUDg/hjj1NeBisonT96lqvxPfwmcQqe7W0Z6jRVSwvIL62Se2lWSGQbg696t/w149nF2Z7UZKSvEWiiigoKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiikPSgBuOOlcv4l8T22ioAGW4vCeLcOAR/vHnHHT1/UY3i7xkIVFrosyvKPme5XDAD0Xsfc9B9ennMsjyyO8zszsSxJOSSepJr2MDlkqvv1NIniY7NVSvCjq+5d1bVrzVpzLfTGQjKqoGAoJzhV/qeeBk8VSpKOtfS0qUKS5YKyPmZ1J1G5Sd2LRRRWxmFFFFABRRRQAlXdCsJNV1GG1iX755PoO7VTr1P4c6J9hsWvZk2zTjjPUJXn4/ErD0m+vQ7svwzxNVLp1Os0+1jsrWOCEbUjUKBVoUUV8Y227s+1jFRXKhaKKKCgooooAzNb0+LVdNmtZQuHHB9D2NeG31rJZXU8EwYPGSpFfQled/EzRd8a6nAnzLxLjuOxr18pxfsqns5bM8bN8J7Wn7SO6POqKKK+sPkwooooAKKKKACiiigC7o+rXmlTtLZTGMnAZSMhgDnBX+o55ODXqfhjxPa60u12FvdZI8gvuz/ALp78dfT9T49T4ZHjZHjYq6EMCDggjoQa8zGZdDErmWkj0cHmFTDO26Poem9q898IeNPPBtdZmRJcZS4OEDD0bsPY9D/AD9CUjHFfKV6FShLkmj6vD4mGIjzwY+iiisjpCiiigAooooAKKKKACiiigAooooAKKKTcKAGABV9BXl/jfxZ9qD6dpcn+jj5ZZVP+s9VB/u+p7/TqeN/Fn2oPp2lt/o/3ZZQf9Z6qD/d9T3+nXiK9/Lct2rVl6I+czLMr3pUn6sWiiivpD54KKKKBBRRRQAUUUUAFFFEYLuqICzE7QB3NDdilqbng3RTq+qhXGbePa8h9R6fjXtMaKihVHCjArB8G6Muj6UsbhftEnzyH39PwroRxmviswxX1irpsj7HLcL9Xpa7sdRRRXCekFFFFABRRRQAlVrq3juYHilAdHUqQe4NWO1HaiL5diXHm0Z4T4j0ptI1SW2fLIrZjJ7g9KzK9c8f6J/aWmfaIUzdW43L7juK8jB5Ir7LLsV7elfqtz43H4T6vVa6PYWiiivRPOCiiigAooooAKKKKACu18EeLPsgXT9TfNsflilJ/wBX6Kx/u+h7fTpxPWlrkxWFhiocsjpwuJnhp88WfQ6tladXlfgfxabXZYao/wDo/wB2KVj/AKv0Vj/d9D/Tp6kmMV8biMNPDT5Jn2WExcMTDmiPooorE6gooooAKKKKACiiigAooooAj+teb/EDxJuEml2Mrbt22dkPGOfk/wAfpjnmtzxx4iOj2flW0kZvZeEU/eRecvj9B7+vSvJK9rKcD7R+2nstjws1x3IvYweotFFFfUnzAUUUUCCiiigAooooAKKKKACuy+G+h/bL1r+4G6GA/JkcF/8A61cpptpJfXkVtbjLyFV/+yr3HRNPi0zTobWL7qDk+prxc2xfsqfso7s9nKcJ7Wpzy2Ro0tFFfLH1gUUUUAFFFFABRRRQAUUUUAMYBlwa8b8daL/ZeqNJEP8AR58unse4r2TIyKxvEulJq+kzW3AlAzGx7N2rsy/FPD1U+j3PPzDC/WKTtujxAUU+eN4JpIpQwdCVIPY02vtYy5ldHxko8vuhRRRVEhRRRQAUUUUAFFFFABXf/D3xIqKml38hznEDseMdk/w+u3jAzwFFcWMwscVT5WdWDxU8LU5kfRANIB1rkfA3iM6xZmG4kQXsX31AwWXs+P0Pv6V14r4yrSdKbhLdH2tCtGtBTgOoooqDYKKKKACiiigBnHSs7V7+DTLCW8uc+XHycDJPPA/PFaPHWvIviDrbahqZs4j+4tGK5BPzv0bI9uQPx55rpweGeJqKC2OLHYlYak59ehzupXc2o3091PtEsjbm2rtAxwAPoABVbtRR1r7WFNU4qMdkfEyqOpJyluxaKKK1JCiiigAooooAKKKKACikFafhjSW1fVIrcA+XuzI3oB1rKrUVKDm+hpSpurNQXU7X4ZaL5MT6lcL80nEW4dF/vfjXoVQW8SQQpFEoCKAoA7Cp6+HxFd16jmz7jC0Fh6aghaKKKxOkKKKKACiiigAooooAKKKKACiiigDy74k6L5U66lbp8khxIAOh7H8a4WvftSs47+ymtphlJF2mvDNWsJNM1Ce1mGGjO0H1HY/lX1GT4v2kPZS3R8pm+E9nU9qtmVaKKK9s8UKKKKACiiigAooooAKKKKALGm3sunX0V1AFMsbbhuGV5GDn6jIr2/R7+DVLGO7tyfLkHGRgjB5B/GvB66r4ea02n6obOVsQXbBeSfkfouB78A/hzxXiZtg/aw9rHdfkexlOM9jU9lLZnsNFIKWvlz60KKKKACiik/hoA5fxrrJ0nR2MLgXcp2RZwfq2Ce388V47W9411j+1tZlZH3W8P7uPByOOp9OT3HUAVg19blWG9jSvLdnx2Z4n21VqOyFooor1jywooooAKKKKACiiigAooooGIK9f8BaL/ZmlLLMMXM4DvnqB2FcP4B0b+09UE0wzbW5DHPQnsK9hAr5rOMXd+xj8z6LJsJvWl8h9FFFeCfRDcUZ60Vla/qsWj6bLcTY44Uep7CnCLm+VbkTmoRcpEup6pa6Zb+beSrGvYHqa4nUfiIQzLY2uV7PIf6CuJ1XUrnVLp7i7kYnPA7AegFU6+mwuTU4K9bVnzGJzepOVqWiOv/4WBqn/ADzh/Kj/AIWBqv8Ach/KuQoru/s/DfyHD/aGI/nZ1/8AwsDVf7kP5Uf8LA1X+5D+VchRR/Z2G/kD+0MR/Ozr/wDhYGq/3Ifyo/4WBqv9yH8q5Cij+zsN/IH9oYj+dnX/APCwNV/uQ/lR/wALA1X+5D+VchSUf2dhv5A/tDEfzs7D/hYGq/3IPyrC13WptYkSW5ijDqMZTuPesyiqpYSjSlzQjZmc8XXqrlm7oWiiiuw5gooooAKKKKACiiigAooooAKKKKBnsHgrWjrGkgzODdRfLJjA+hwD3/mDXT7RzXingnV/7J1lC77beb93Jk4HPQ+nB7noCa9rUgjNfE5hhvq9V22ex9nluJ+sUVfdD6KKK4j0RnHFc94u1Y6Ro0twnE7/ALqI5/jP8XfoMnn0x3roMivLPibqTT6pHZRviK3QMyjP32/vdjgYx/vGurA0PrFaMehxY+v7GjJ9ehxlFFFfcpWPiAooooEFFFFABRRRQAUUUUAFOtoXuJkhiDF3IUAdyabXe/DPRPMlfU7hPlX5Yc9/Vq5MZiFh6TmdWEw7xFRQOy8MaSmkaVFbIPmxlz6t3rZNAHFID1r4ic3OTm92fb04KnBQQ8UUUUjQYeBn0rx/x7rR1PVGgibNtb8DHQnua7jx1rX9l6W0cLYuJxtTb1Hqa8fzltzda97JsJe9aXyPnc4xf/LmPzFooor6U+cCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAK9m8Gat/a2ixTyA+en7qU56uP4ugHI2nj1x2rxiuz+GGpNBqstjJJ+5uE3Kpz99fTtyN2foK8bN8P7SjzreJ62UYj2VblezPWKKKK+VPrytNKkETyysFjUZJJwBivBtUvHv9RuLqTcGlkL4Y5wC3Az7DA/CvXPHt99i8NXPTfOPJUMpOd3B6f7OT+FeNV9DkdHSVV+h81ndbVUl6i0UUV9EfPhRRRQAUUUUAFFFFABRRRQMvaLp8uqajDaw/8ALQ8n0Xu1e5adaR2NnFbwrtjjXaBXI/DrRfsVib24TFxcDjI5Cf8A1/8ACu3Wvj8zxft6nJHZH1uVYT2NPnluxaKKK8w9cSoJpVgheSQ4RQSSanrz74l615MI023f55FzIR2HYfj/AErbDUHWqKETmxNdYem5s4vxPqz6zqktwSfKB2xj0A/zn8ayqOtLX29KkqUFBdD4erUdWbm92FFFFbGYUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFACDpVnSrt7G/t7qPduikD4U4yA3Iz7jI/Gq9JWVSmpxcX1NKc3GSkt0fQVvMk0SSRMHRxkMDkGisHwFffbfDltnG+EeSwUEY28Dr/s4P40V8FWg6c3E+9w81UpqRznxXusGytFmXaQ0jxYGewDZ6j+P6/N6V5/XTfEW58/xNMmzb5KJFnPXjf8Ah9/H4VzHavsMsp+zw8fM+NzGp7XESfYWiiivSOAKKKKACiiigAooooASt3wZox1fVEUr/o8fzyH29PxrDjUu6ooYsTtAHevaPBmjLo+kJG4X7RJ88h9/T8K8vM8X7ClyrdnqZZhPrFTXZG9GgRAqgBQMDFSUUV8gfYrQKKKSgDP1jUItM0+a7nPyRr+deWW+gar4kefUfkXzXJG89fp7Ve+JGuC7vFsLd90MB+fHRn/+tWv8MNVE1k9hK372H5k91P8Ah/Wvbo06mEw3t4LV/keDWq08XifYTei/Mwv+EA1T+/b/APfR/wAKo6p4Q1PT7Vp3RJETlvLOSB64r2fimSoskTIw+UjGKxhnGIT12NpZPQ5Xynz3RWp4m006Vq9xb4OzdujPqD0rKr6ilUVWCnHqfLVYOlNwl0Oj0rwfqOoWSXMexEfkbzgketXP+EB1X+/b/mf8K6n4b6oLzSPsztme2+XH+z2rsM5yBXzeIzLE0qjh2Po8NluGrU1PueLax4U1DSrbz5lV4l6mM9PrWAK+gbu3S6tpYZRuRwQRXhOr2bafqFxauOY3K/8AAe36V6OWY94m8Km552ZYBYZqUNirGC52KGLE7QB3NdTb+BdUlhWRhFGSOhPIo+HOlfb9X+0OMw243e2ew/rXrqgcisMwzKdGpyUzoy7LIVqfPU+R5P8A8IBqh/jt/wAz/hWLrugXeilBdhdknRwcgt/dr3QkAV5P8S9W+16klnEcpb9f98//AFqzwGPxFesoPY0x+Aw+HpOa3OPp1vDJcTJFCjO7naAOpNMrufhjpXm3Mt/IvyR/JH9T1P5V7WMxCw9JzPGwmHeIqKBTi8Bas6KSYUJHQnkU7/hX+q/37f8AM/4V6z+VHbtXzX9r4jufS/2Ph/M8J1rR7nR7jyrsAZHBByCKza6Tx9qo1HWHSI5gg+QEdz3Nc2K+lwc5zpKVTc+ZxMIQquMNkLRRRXUc4UUUUAFFFFABRRRQAUUUUAegfCi5yb+1aZdo2SJHxu7gn/0D6cetFYnw6ufI8Som3PnRtF9ON/4/cx+NFfG5nRtiJeZ9blmJ/cJGZ4mnkudf1F5DlvPdBx2DYA/ILWZT5pWuJpZZjl5HLucYyS2SeKZ3r6uhT9nTUOx8vVnzzb7sWiiitzIKKKKACiiigAooqfT7WW8vIre3GXkIA/8AiqiUlFc0ioxdSXLE6r4baL9svmvpk/c25+TPQv8A/Wr1cAY4rP0TT4tM0+G1h+6g6+p71oA8V8TjMS8RVc+h9tgcMsNSUOo6iiiuU7SMe9YXi/WF0jSZJFI8+T5Ix7+v4VuMQqkk4A614v4z1g6vq8hRs28XyRj1Hc/jXdl2F+sVddluedmOK+r0tN2YbuZHZ3LFnO4k92q94fv30rVLe6Q/Kh2keoPWs8UCvr50lODg9j4+FRwmp9UfQdtMk8CSxHKOAQfap64X4Zat9psXsZX3SQcpnun/AOuu6r4evRdGo4M+5w1ZVqamjh/iVpP2nTVvYhult+uP7hry32r6EuIUmheJxlHGCK8L13Tm0vVbi1deEPB9R2Ne9kuJ5oujLpseDnWFtJVY7dS14Q1Q6TrMUpbEDnZIPY9/w617ajBlypyD3r54r1/4f6v/AGloywu+Zrf5DnqR2NZ5zhv+X0fmXkuJ3oy+R1HevKviabY60gi/4+An7zHT2/GvS9SvI7GymuZThI1LGvKPDlpL4i8UNNMCU3+dIW9M/KtcmWLkbrvaJ15m+dRoLds77wNpZ03Q4xIMSy/O/rz2rpQOtC4UYpa86rN1Zub6np0aSpQUF0MnxBqSaXpVxdN1VcKPU9q8OnkaeaSVzl3JYn1JrtPibqv2i8SwiOY4fmk57/8A1hXE19Nk+G9nS55bs+XzfE+1q8i2RJbQvczpDEGLuQoHua9z8P6fHpmlQWkf8I5Pqe9ed/DPSftOovfyDMUHCZ7uf8BXqo/lXnZxieefslsj0clw3JB1X1Ex+lc/4z1b+ydFlkQ4mk+SP/ePf8OtdD0FePfELV/7R1doY2zb2/yD0Ldz/SuPLsN9YrKPRHZmGJ+r0W+rOYZi5YscsfmNFFFfbpWPjAooooJCiiigAooooAKKKKACiiigDR8NTSQ69YNCcN5yIeOxOCPyJoqhbzPBNHJEcSIwdTjdgg5B5orx8bgZ1580Tvw2JVKPKxtFFFewcAUUUUAFFFFABRRRQAnSvR/hnovlwtqdwvzvxFkdB3Ncb4Z0ptX1SK2GdmcyN6Ada9utYkggSKMBUQAADtXgZvi+SPsY7vc93J8Jzy9tLboWKKKK+bPqBKSlqtf3UdlayzTHakY3E0RTbsTKSiuZnJ/EbWfsOn/Y4W/0i4G046hO9eVAck1f1zUJdV1Ge6lLfOfkHoOwqjX2eX4VYelbq9z4rHYl4mq306BRRRXoHCaPhzUm0nVoLoZ2g4dfUHrXuVvMk0KSIco4yDXz33r1L4aav9q01rCVsy2/TPdK+eznC3SrR+Z7+S4m03Rlt0O4rgPidpPn2yahCPni+WTHdfX8P6139V723ju7WWGVQyOCpBrw8NWdGopo93E0VWpuDPn6t7wTqv8AZWtxF3xDL8knpg9D+FZer2Mmm6jcWsobdGSoPqOx/Kqi8HK19nOEcTSt0Z8XCbw1S/VM9E+J2rYSPToW+/8APJg9uw/rW54B0caZo4kdcXFx87+3oK4TwpZz6/r6NdsZEiCvIT3A6D+VewoFWPHYV83jWsPSWGj6s+jwF8TVeJkvQdxj6Vn63qCaZps90/8AAvA9T2rR4xXmXxO1bzbiPTYj8sfzyY9ew/z61x4Sh9YqqB3YzEfV6Tn1OIuZ5Lm4lmmcl5CWJ9SabChkdUQMXJ2gDuaZXW/DjSfturG6lGYbbpkdX7fl1r6/EVFhqLn2Pj8PTeIrJdz0Xwzpo0rR4bYfeAy59T3rW7ml4Ao6CviJyc5OUup9vTgqcFBdDC8YaqNK0WaZSPNf5I/9414mWLMxYsWPzEmuq+IOrfb9XaCNsw2/yD0Ld65Wvq8pw3saXM92fKZpifbVXFbIWiiivXPKCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACkpa6bwBov8Aaeq+dOM2tuQxz0Zu1c9euqFNzl0N8PRdaooLqdx4A0b+y9M82ZcXM4BfPUDsK6rHJoAAXAo7V8RWqurNzl1PuKFFUaagug6iiiszYbXm3xM1re66ZbnhcPKQfyFdn4j1SPR9LnunwWAwg/vN2FeIXM8lzPLNM5d5CWJPcmvYynCe1n7SWyPDzbF+zh7KO7GUUUV9WfLhRRRQIK0vDWpHS9Xt7kE7A21x6g9azKKxqUlUg4PqaU5ulNTW6PoSCRZYlkQ5QgEEVL61xPw01b7XpzWkz7pbf7u7qU7fl0rtxXw+IoujUcJdD7rDVlWpqaPOfihpAKR6lEOhCSY9Ox/pXnle+apaR31jNazDKSIVNeT+HfD8lx4nayuE+S1fdJ/ujoPxr3ctxyjQlGb+E8DMsC5V1KC3O68AaSdN0VZHGJ7jDt9Ow/KuqxnNCqFUCl7V4FWo6tRzl1PoKFJUaaguhQ1a8TTtPuLqQjbGuee5rw68e4vpri9cMVL7nfsCei12/wAT9VLPFpcDf7cgH6D+ta+k+GIx4TeymwJrhd7N/db+H8uK9jBTjgaSqz3k/wADxsZCWOqunDaP5nlEaFmVVG5idoHrXtXhLSl0nRoIePNYb5P94/5xXn3gjQnudff7THhLNvnz/eB+Uf1/CvXkGBgUs4xftGqcGGT4XlvUkhMDjnpWH4u1QaVo08yn9642R/7x7/h1rdxgfSvIviLq/wBu1U20TZhtuPbd3/wrhwGH+sVlHp1O/McT9Xot9WcsxJZnY5YncTSUUV9ulY+MCiiigkKSgdKt6NYSanqENrCMtI20n0XufyrOdRQi5PoVTg5tRjuy3a6Dc3GiXGpIv7qI9Mcle5H0rJr3yxsIbTT0tI1HlIuzFeO+LdHOjatLEB/o7/PGfb+7+FeTgMx9vUlCXyPVxuXPD01OPzMaiiivaPICiiigAooooAKKKKBlnVYFt9VvYYRhI5nRBknADMAOfpVUd62/G0Edv4o1BIl2rvDnk9SAS35k1iDvXNhZc9KL8kbYiHJUlHsxaKKK6TAKKKKAHW0LXNwkUQZnchQB3avbvDGkpo+lRWw5fGXPqe9cV8MtF82ZtTuF+SP5YQfXu1enV8tm2L9pP2UdkfU5PhOSHtpbsWiiivFPcGkc0ZwpNLXK+O9aGl6W0cb7biYFE9R6mtKNN1ZqEepjWqqjBzkcT4+1r+09Ta3hb/R7clRju3c/0rlqMluW5aivt8NRVCmoI+HxFZ1qjmwoooroMAooooAKKKKANTwvqZ0nV4Z8/JnbIPUHrXuELrJGrocqRkGvnrHNer/DbVvtulmzlfM1t8v/AADtXz2c4a6VaPzPoMlxNm6MvkdnVWKzgjuJZ0jVZZcb27nHSrQor50+ksmIBVXULqOxspp5ThEXcatV578T9W2xR6dE/wAz/PJ/u9q3wtB16igjmxddUKTmYnhe1k8ReJ3u7kZRD5jenX5Vr1vov0Fcd8M4bdNEMkJBldv3nqCOg/KuyA4rbMKnNVcFtHQwy2ny0efqytaWUFtJPJDGqPM29yO5q3SdaOgrivfc74pRWhjeK9UTStGmnyPMI2Rj1Y14g7l3Z3ZixO4k92rrPiNq323VfssTZituuO7965KvrMpw3saXM92fJZrifbVeVbIWiiivXPJCiiigBK9R+G+ifZLH7dMv76cfJnsn/wBeuK8HaOdY1ZIyG+zx/PIfb0/Gvao0CKABgAYr53OMXZexj8z6DJsJdutLboPFc3400X+19KYRgfaIvnjP9PxrpM0mODXgUqjpTU49D6CrTVWDgz54YFCwYYYHaQaSut+IWi/YNR+1wjFvcHccdA/f8+tclX3GFrKvTU0fD4ii6FRwYtFFFdJzhRRRQMs6PAtxq1nFMMxSTIjjJGQSARxRWj4JgSfxNYpIm5Q5cDnqASG/MCivnM0rSjVSTtoe1l1BTpttdTa+Klsy6vaXGRski2KO+Q3P/oa/rXFV6j8UbLztGiuVjyYJBk7sYRuPx+bZXl9d2UVefDpdjDNaXJiH5hRRRXqnliVd0Swk1XUIbWH70h5PoO7VTr1H4b6J9jsWvpkxNP8AcyOQlefjsSsPSb69Duy/DPE1UunU63TbSOxs4reEYSNQoq2KKK+Mbbd2faxiorlQtFFFBRBM6wxO8hwg5JrxHxPqzatqss+W8pfljHoK9R8dzGHw1ebONy7D+NeMV7+SUFrWZ85nVZ3VJC0UUV9IfPBRRRQAUUUUAFFFFACCtfwpqZ0nV4J8nyidkg9QayaTpWNWkqsHB9S6U3SmprdH0LG6yRq6EEEZFSAda474d6t9t0n7NK+6a3+XnqU7f4V2VfDV6bo1HCXQ+6w9ZVqamireXEdtbSzSNhI1LE14ZrF82o6jcXUp/wBYSR7DsK774m6t5VqmnxN88vzSY/uj/wCvXmgr6HJsNyRdaXU+dzjE88/ZR2W51vw51b7DqxtZTiG54/4H2/wr1tTxXz1E5jlVkLBgQwYdjXt3hfVF1XR4bkH58bXHow61y5zhuWarR6nVkuJ5k6L6GvkdayPE2qLpOkT3P8eMIPVu1a/AFeTfEfVvtmpLZwnMVv8Aex0LmvPwGH+sVlHp1PRx+J9hRb6nJu5kkZ3JLE7iTTaKK+4SsfFbhRRRQSFCqXO1QxZjtAHeiuu+HWi/btQ+2zjMNu3APd+35da5cViFh6bmzowtF4iooI7fwXov9kaSqOF+0SfPIff0/CujpMdBRnrXxFWbqyc5bs+4pUlSgoRH0UUVJqZWu6bFq2lz2knAccH0PY14feW8tpdSwTDEsZKke4r6DFedfEzRSQNTt0+YYSUD07H+levlOL9lP2ctmeJm+E9rT9pHdHnlFFFfWHyoUUUUAdn8Krd31a7nyuyOLYw75Lcf+gN+lFbnwus/K0ae5ePDzyHB3Zyq8fh82+iviswrKWIkz7LLcO1h4nT65afbdIu7UBN0kZVd4yA3Yn8a8I719EnkYrxPxxYGx8RXgwdkp85M478n9d35V25JW5Zum+px53R5oKouhh0UUKCWAUEsTtAHevqL2Pm1qbng3SDq+rJGR/o8XzyH29Pxr2mNQihFGAo4rB8GaMuj6SiOB9ok+eQ+/p+FdAMivisxxX1irpsj7HLML9Xpa7sfRRRXCekFFFFAGX4gsf7Q0e6tR96RDg+9eEyxPDNJG6MroSpB6g19EVxvi7wdHqjtd2JEV2fvA9H/AMD716uWY5YduE9mePmmBeISnDdHlHNFa9z4Z1a3dg9jKf8AaQZBqL+w9U/58bj/AL4NfSxxFFq6kj5l4atF2cWZvNHNaX9hap/z43H/AHwaP7C1T/nxuP8Avg0/rFL+ZB7Cp/KzN5o5rS/sLVP+fG4/74NH9hap/wA+Nx/3waPrFL+ZB7Cp/KzN5o5rS/sLVP8AnxuP++DR/YWqf8+Nx/3waPrFL+ZB7Cp/KzN5o5rS/sLVP+fG4/74NH9hap/z43H/AHwaPrFL+ZB7Cp/KybwlqjaTq8Uxb9yTskHse/4da9qluEitmmkZRGBuz7V4b/YWqf8APjcf98GtWVvEkmnfYXiuzbgYxs5YemfSvIx+Fp4manGa8z1cBiqmGg4Sg/IyNe1B9V1Se6fPzn5B6DsPyqj2rS/sPVP+fG4/74NH9h6p/wA+Nx/3wa9SnVowgoxkrI8ydKtOblJO7M3muy+Geqm01F7KRv3dx0yejj/Guf8A7D1T/nwuP++DSw6Nq0Mgkis7kOh3AgHINY4p0cRScOZGuFVbD1FPlZ6/4i1NNL0qe5J+YDao9WPSvDpZHmmkkc5dyWJ9WNdDqi+ItTRFvILmRI+g2bRn1+tZ/wDYeqf8+Nx/3wa5suo08LF80ldnVj61TFyXLF2Rm80c1pf2Fqn/AD43H/fBo/sLVP8AnxuP++DXpfWKX8yPO9hU/lZm4orUTQNVc4FjcH/gFbGleBdSuSDc7beLuTyfyrOeNowV3IqlhKs3ZQZz2lWE+pXsVtbIxdz17Bf7xr23QtOi0rT4rWEcIOT6moNA0O00W38u3XLn78h6mtj1r5nH4/61Ky+FH1GXYD6tHmnux1FFFeceoFFFFADetV7u3S5t5IZhuSRSCD6VZpaI+7qiXHmVmeDeINMk0nU57ZwdobKN6g9DWb1r1z4g6L/aWlm4hGbi3BYY6le4ryMdTX2OXYn6xSu90fF4/CfVqrXR7C0UVueB9PN94jtVwdkR858Y7cj9dv511V6qpU5TfQ5cNSdSah3Z6zoNl9h0m1tSEDRxqGKDgtjkj8aK1KK+Dk3J3PvoRUI8qEFcD8UNOE9hFfIvzwuEcg4+Q/z5xj6mu+zVe8to7q2mgmG+ORCjj1BGDW2GqujUU10McTRVam4M+fq7H4caJ9uv2vphmC3Py57v/wDWrnDpVyur/wBmlP8ASPM8raAcH3Ge2OfpXtWiadFpenRWsPRByfU+tfR5njVGlywesvyPnMrwTnWbmtEaQpaKK+XPqwooooAKKKKACiiigBKNopaKAsJtFG0UtFAWE2ijaKWigLCbRRtFLRQFhNoo2ilooCwm0UbRS0UBYTFG0UtFAWE2ijaKWigLCbRRtFLRQFhNoo2ilooCwm0UtFFABRRRQAUUUUAFFFFABRRRQAzjGK8b8c6J/ZOrM8Yxa3GXT2PcV7IB0rG8T6Qur6TLb4Hmj54z6Gu3L8V9Xq36Pc8/McL9YotLdbHiNel/C7TPK0+W/dfnmcojH+4v8uc/kK89tbGefUI7KNG85pNm0g8Nu5Jx2HJPsK92sbeO1tooIU2RxoqKPYdK9bOMVaCpR6nkZLhbzdSXQt0UUV84fThRRRQBzlzoif8ACSW+poAcRFHHTLcYPTnjcOT6V0HrSnpWdDqNs9/Naecn2mL7yZ5HcfoapuU16GKjGlfzNKjFJuHqKNw9RU2NeZC4oxSbh6ijcPUUWDmQuKMUm4eoo3D1FFg5kLijFJuHqKNw9RRYOZC4oxSbh6ijcPUUWDmQuKMUm4eoo3D1FFg5kLijFJuHqKNw9RRYOZC4oxSbh6ijcPUUWDmQuKMUm4eoo3D1FFg5kLijFJuHqKNw9RRYOZC4oxSbh6ijcPUUWDmQuKMUm4eoo3D1FFg5kLijFJuHqKNw9RRYOZC4oxSbh6ijcPUUWDmQuKMUm4eoo3D1FFg5kLijFJuHqKNw9RRYOZC4oxSbh6ijcPUUWDmQuKMUm4eoo3D1FFg5kLijFJuHqKNw9RRYOZCZ9qX+GkLgdxWLe61brdxafbyh7uc4AQg7B/E34CmoN7EOcVuzI8P6On/CT6rqbL8iyeVBkfxbRvIyPXjIPqK7Ko4kSKMRoAEA4FSVU5uo7sVGmqatEdRRRUGoUUUUAJXF+PNEk1G1W9s1b7bbjAEYGXXPIz1yOSPx45rs80EZUirpVHTkpoxrU1Wg4M8B/tXUB92+u/8Av8f8ad/amof8/wDd/wDf5v8AGtzx34fbS7xru2RFsp3+UDjy3+8Vx74yPy4/i5avscM6OIpqaSPjMQq2HqODbLv9qah/z/Xf/f4/40f2pqH/AD/Xf/f4/wCNUqK39hT/AJUYe2n/ADMu/wBqah/z/Xf/AH+P+NH9qah/z/Xf/f4/41Soo9hT/lQe2n/My7/amof8/wBd/wDf4/40f2pqH/P9d/8Af4/41Soo9hT/AJUHtp/zMu/2pqH/AD/Xf/f4/wCNH9qah/z/AF3/AN/j/jVKij2FP+VB7af8zLv9qah/z/Xf/f4/40f2pqH/AD/Xf/f4/wCNUqKPYU/5UHtp/wAzLv8Aamof8/13/wB/j/jR/amof8/13/3+P+NUqKPYU/5UHtp/zMu/2pqH/P8AXf8A3+P+NH9qah/z/Xf/AH+P+NUqKPYU/wCVB7af8zLv9qah/wA/13/3+P8AjR/amof8/wBd/wDf4/41Soo9hT/lQe2n/My7/amof8/13/3+P+NH9qah/wA/13/3+P8AjVKij2FP+VB7af8AMy7/AGpqH/P9d/8Af4/40f2pqH/P9d/9/j/jVKij2FP+VB7af8zLv9qah/z/AF3/AN/j/jR/amof8/13/wB/j/jVKij2FP8AlQe2n/My7/amof8AP9d/9/j/AI0f2pqH/P8AXf8A3+P+NUqKPYU/5UHtp/zMu/2pqH/P9d/9/j/jR/amof8AP9d/9/j/AI1Soo9hT/lQe2n/ADMu/wBqah/z/Xf/AH+P+NH9qah/z/Xf/f4/41Soo9hT/lQe2n/My7/amof8/wBd/wDf4/40f2pqH/P9d/8Af4/41Soo9hT/AJUHtp/zMu/2pqH/AD/Xf/f4/wCNH9qah/z/AF3/AN/j/jVKij2FP+VB7af8zLv9qah/z/Xf/f4/40f2pqH/AD/Xf/f4/wCNUqKPYU/5UHtp/wAzLv8Aamof8/13/wB/j/jR/amof8/13/3+P+NUqKPYU/5UHtp/zMt/2nfOGU3tyc9jKf8AGvSvAOhPptm15dqRd3HVXA3Ivp9TwT+Ax8tcp4C8PHVb37VcopsoX+ZTz5j9QuPbIJz9OecevgALivns0xML+xpfM+hyrDTl++q/IdRRRXinvhRRRQAUUUUAFFFFAFO/tIb60ltrmNZIZBtKN/FXiOvaPc6LetDdLlTny5VGFkX+h9R2+mCfePasbxHo0Otae1vKSjA7o5B/A+CA3v1rvwGMlhp67Pc83H4FYmF1ujw+irerabc6XdyW12mHX5gR0cdiPUf561Ur6+nUjUjzR2PkJRcG4yVmhaKKK1MwooooGFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAlaPh7RrnWbxYLZcKMeZKwysY/qfQd/pkiHS9PudTu0trVcueST0QdyT2H+etey+HNFg0TTxbRMWOd8jn+N9oG726V4+Y49UI8kPiZ6mWYGVefNL4EX7CzhsbZLe2jWOGMbQi9qt/w0lLXyl23dn10YqKshaKKKCgooooAKKKKACiiigAooooAwPEug2+t2Zjm4kX5o5QuSh/qPavHtU0+50u7e2u1w4+YEdHHYj1H+ete/dqw/EegW2t2Zjm+WVfmjlC5KH+o9q9HAY+WGlyy+E8rH5csQueG54lR0rR13RbrRboQXYX5xuR0J2P69u3cf4is6vrKNWFWPNB3R8nODpNxkrNC0UUVsZhRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAlW9I0+51O7FtZplzyT2QdyfT/PepdC0W61q6MFkASo3OzMVRPTJwevYf4GvXfDWg22iWflw/NK2GklIwXP9B7V4+PzKNBcsNZHqYDLp4mXNLSH5ieG9Bg0S0EcPzytgySEYLH+g9q3QOOKBjpSV8rOcptzmz62lSjTjyQ2H0UUUjQKKKKACiiigAooooAKKKKACiiigAooooAzNT0y11K0aC7iEsbHOGJGD7eleVeKPCtzo2+dP39kXwHH3kH+3/Lj9Oley9qQqGHNdWExtTDP3duxw4vA08Sve3Pnj60V6V4n8CxTCS40gLFKASYP4JDnPHp9On06151dWs1lcS29zG0cycFD/AJ/WvqsLjqeJWm/Y+VxOBqYZ2a07kdFFFd5xBRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUVJZW0t3cpb20bSSvwAP8/rUykoq8ioxlJ8sSOuh8LeFrnWSs8n7iyD8u3Vx/sfyyePrjFdR4X8DQwBbjWAssxAYQfwxndnk9/p06/e613qqFGFr53GZve8KH3nv4HKHpOt9xQ0nTrXTLUW9nEIowc7QSefqetaK89aKK8Btt3Z9FGKirRQ6iiigoKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigBprM1XSbLU4Sl7bpMMcE/eX6Ecjt0rTAOKWmm4u8SJRU1aSPJNe8DXtiHmsD9qgGWx/wAtF69u/Qfd5JPSuRljeNnjkQq6EggjBBHUEV9EbRisfVND0/UwGvbVJGXo3Ibv3HOPmPFevhc4nD3aqueJiclhLWi7Hh9Fdvq3w+uInLaVOJkAz5cx2uOPuggYbJ9cY4rkL3T7qxlKXltNE2SoLggHHXB6H6iveo46jX+Bnh1sFWo/GvmV6KKK7TlCiiigAooooEFFFFABRRRQAUUUUAFFFFABSYoqzY6fdX0uyztpZmBCkoCwGemT0H1NZVKkYK8nY0p05Sdoq7K9EUckkiRxoXdyAABkknoAK7fR/h/cyyK2qziGPCtsiO5+h+UkjC4OOmc8/Wu50jQ9P0rcbK1jjd+r9WP4nnHA4rysRnFKnpT1Z6mGyarU1q6I890LwPfXuyXUD9kgODg/6wj5T93oOM9eQR0r0fSNKstMhMdlbRxD+Ijkt9T1PfrWkANp5pMcda+fxGMq4h++/kfQYbA0sMvcWpJRQKK5jtCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACq80Mc0bxyRq8bjaQRkEHrViijYLHJaj4J0m93ssTW0hIbfCcdBj7pyv6VzV/8PLkOTYXUToT0mG0hewyM5/IV6cacvSuijmNek7RkcVbLaFXWSPD7rwvrVtGWk06cqTjEZDn8gSe3WsqeGa3kaKeN4pR1RwQfXkGvoNaZIBzxXpUs7qP4oo82rktOPwyZ8+UlexSaPpnzf8S6z/78L/hXj1e5hcS617o8GtR9nswooortOcKKKKBiU+CGWeQRW8byOeiICT68AV7JFo2mcf8AEtsv+/C/4VsxgeleFVzZx2j+J61HLYz3keMWnhfWLmPdFp0wXpiQhD+RIP410Wn/AA8upG3X93EiAjiIbiV7jJAx+Rr0wClFeZWzfES2dj1qGU0I76nK6b4K0qyKu0L3Lgn5pznqMfd4X9K6SGGOCJY4o1VFGAAOgqcU0158q1SpK83c9KFGnRXuIkooFFQbBRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB//2Q==";

        public static bool GradleBuildEnabled {
            get { return GetEditorUserBuildSetting("androidBuildSystem", "").ToString().Equals("Gradle"); }
        }

        public static bool GradleTemplateEnabled {
            get { return GradleBuildEnabled && File.Exists(GradleTemplatePath); }
        }

        [MenuItem("HW Games/SDKBundle/SDKBundle Settings", false, 300)]
        private static void EditSettings() {
            Selection.activeObject = CreateSDKBundlettings();
        }

        [MenuItem("HW Games/SDKBundle/Create SDKBundle Object", false, 200)]
        static void AddGASystemTracker() {
            if (UnityEngine.Object.FindObjectOfType(typeof(HWSDKBundleBehaviour)) == null) {
                GameObject go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(WhereIs("HWSDKBundle.prefab", "Prefab"), typeof(GameObject))) as GameObject;
                go.name = "HWSDKBundle";
                Selection.activeObject = go;
                Undo.RegisterCreatedObjectUndo(go, "Created HWSDKBundle Object");
                EditorSceneManager.SaveOpenScenes();
            }
            else {
                Debug.LogWarning("A HWSDKBundle object already exists in this scene - you should never have more than one per scene!");
            }
        }

        private void Awake() {
            environmentValueStyle = new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleRight
            };
            LogoTexture = new Texture2D(80, 80, TextureFormat.RGBA32, false);
            LogoTexture.hideFlags = HideFlags.DontSave;
            LogoTexture.LoadImage(System.Convert.FromBase64String(Logo));
        }

        private static SDKBundleSettings CreateSDKBundlettings() {
            SDKBundleSettings settings = SDKBundleSettings.Load();
            if (settings == null) {
                settings = CreateInstance<SDKBundleSettings>();
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    AssetDatabase.CreateFolder("Assets", "Resources");

                if (!AssetDatabase.IsValidFolder("Assets/Resources/SDKBundle"))
                    AssetDatabase.CreateFolder("Assets/Resources", "SDKBundle");
                AssetDatabase.CreateAsset(settings, "Assets/Resources/SDKBundle/Settings.asset");
                settings = SDKBundleSettings.Load();
            }

            return settings;
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.BeginVertical();
            DrawLogo();
            Space(2);
            EditorGUILayout.LabelField("HW SDKBundle version " + SDKBundle.Version, EditorStyles.boldLabel);

            Space(2);
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("Facebook", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            sdkBundleSettings.facebookAppId = EditorGUILayout.TextField("Facebook App Id", sdkBundleSettings.facebookAppId);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5f);
            using (new EditorGUILayout.VerticalScope("box")) {
                SelectableLabelField(_ClassNameLabel, DeepLinkingActivityName);
                SelectableLabelField(_EditorAnalyticsID, sdkBundleSettings.EditorIdfa);
            }

            GUILayout.EndHorizontal();

            Space(2);
            EditorGUILayout.LabelField("GameAnalytics", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            sdkBundleSettings.enableGameAnalytics = EditorGUILayout.Toggle("Enable GameAnalytics", sdkBundleSettings.enableGameAnalytics);
            if (sdkBundleSettings.enableGameAnalytics) {
                GUILayout.Space(5f);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                sdkBundleSettings.gameAnalyticsAndroidGameKey = EditorGUILayout.TextField("GameAnalytics Android Game Key", sdkBundleSettings.gameAnalyticsAndroidGameKey);
                sdkBundleSettings.gameAnalyticsAndroidSecretKey = EditorGUILayout.TextField("GameAnalytics Android Secret Key", sdkBundleSettings.gameAnalyticsAndroidSecretKey);
                Space();
                sdkBundleSettings.gameAnalyticsIosGameKey = EditorGUILayout.TextField("GameAnalytics IOS Game Key", sdkBundleSettings.gameAnalyticsIosGameKey);
                sdkBundleSettings.gameAnalyticsIosSecretKey = EditorGUILayout.TextField("GameAnalytics IOS Secret Key", sdkBundleSettings.gameAnalyticsIosSecretKey);
                EditorGUILayout.EndVertical();
            }

            Space(2);
            EditorGUILayout.LabelField("Adjust", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            sdkBundleSettings.enableAdjust = EditorGUILayout.Toggle("Enable Adjust", sdkBundleSettings.enableAdjust);
            if (sdkBundleSettings.enableAdjust) {
                GUILayout.Space(5f);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                sdkBundleSettings.adjustAndroidToken = EditorGUILayout.TextField("Adjust Android App token", sdkBundleSettings.adjustAndroidToken);
                Space();
                sdkBundleSettings.adjustIOSToken = EditorGUILayout.TextField("Adjust IOS App token", sdkBundleSettings.adjustIOSToken);
                EditorGUILayout.EndVertical();
            }

            SetEditorIDFA();
            DrawUnityEnvironmentDetails();

#if UNITY_IOS || UNITY_ANDROID
            Space(5);
            if (GUILayout.Button(Environment.NewLine + "Check and Sync Settings" + Environment.NewLine)) {
                CheckAndUpdateSdkSettings(sdkBundleSettings);
            }
#else
            EditorGUILayout.HelpBox(BuildErrorConfig.ErrorMessageDict[BuildErrorConfig.ErrorID.INVALID_PLATFORM], MessageType.Error);   
#endif

            Space(5);
            if (UnityEngine.Object.FindObjectOfType(typeof(HWSDKBundleBehaviour)) == null) {
                EditorGUILayout.HelpBox(BuildErrorConfig.ErrorMessageDict[BuildErrorConfig.ErrorID.NOHWPREFAB], MessageType.Warning);
            }
         
            EditorGUILayout.EndVertical();
    
            if (GUI.changed) {
                SaveChange();
            }
        }

        private void CheckAndUpdateSdkSettings(SDKBundleSettings settings) {
            Console.Clear();
            BuildErrorWindow.Clear();
            FacebookPreBuild.CheckAndUpdateFacebookSettings(settings);
            if (sdkBundleSettings.enableGameAnalytics) {
                GameAnalyticsPreBuild.CheckAndUpdateGameAnalyticsSettings(settings);
            }
            if (sdkBundleSettings.enableAdjust) {
                AdjustBuildPrebuild.CheckAndUpdateAdjustSettings(settings);
                SDKBundle.UpdateAdjustToken(settings);
            }
            if (Application.identifier.Contains("DefaultCompany") || Application.identifier == "") {
                BuildErrorWindow.LogBuildError(BuildErrorConfig.ErrorID.PACKAGE_NAME_ERRROR);
            }
        }

        private static void Space(int count = 1) {
            for (int i = 0; i < count; i++) {
                EditorGUILayout.Space();
            }
        }

        private void SelectableLabelField(GUIContent label, string value) {
            if (_Style_FBNativeAndroidAppSettings == null) {
                _Style_FBNativeAndroidAppSettings = new GUIStyle(EditorStyles.label);
                _Style_FBNativeAndroidAppSettings.fontSize = 10;
                _Style_FBNativeAndroidAppSettings.normal.textColor = GUI.contentColor;// Color.white;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, _Style_FBNativeAndroidAppSettings, GUILayout.Width(180), GUILayout.Height(16));
            EditorGUILayout.SelectableLabel(value, _Style_FBNativeAndroidAppSettings, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawUnityEnvironmentDetails() {
            Space(5);
            EditorGUILayout.LabelField("Unity Environment Details", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            using (new EditorGUILayout.VerticalScope("box")) {
                DrawUnityEnvironmentDetailRow("Product Name", Application.productName);
                GUILayout.Space(5);
                DrawUnityEnvironmentDetailRow("Package Name", Application.identifier);
                GUILayout.Space(5);
                DrawUnityEnvironmentDetailRow("Version", Application.version);
                GUILayout.Space(5);
                DrawUnityEnvironmentDetailRow("Unity Version", Application.unityVersion);
                GUILayout.Space(5);
                DrawUnityEnvironmentDetailRow("Platform", Application.platform.ToString());
                GUILayout.Space(5);
                DrawUnityEnvironmentDetailRow("Gradle Template Enabled", GradleTemplateEnabled.ToString());
            }

            GUILayout.Space(5);
            GUILayout.EndHorizontal();
        }

        private void DrawUnityEnvironmentDetailRow(string key, string value) {
            using (new EditorGUILayout.HorizontalScope()) {
                GUILayout.Space(5);
                EditorGUILayout.LabelField(key, GUILayout.Width(250));
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(value, environmentValueStyle);
                GUILayout.Space(5);
            }
        }

        private static object GetEditorUserBuildSetting(string name, object defaultValue) {
            var editorUserBuildSettingsType = typeof(EditorUserBuildSettings);
            var property = editorUserBuildSettingsType.GetProperty(name);
            if (property != null) {
                var value = property.GetValue(null, null);
                if (value != null) return value;
            }

            return defaultValue;
        }

        private void DrawLogo() {
            GUI.DrawTexture(new Rect(40, 40,70, 70),LogoTexture, ScaleMode.ScaleToFit, true);
            GUILayout.Space(70+50);
        }

        public static string WhereIs(string _file, string _type) {
#if UNITY_SAMSUNGTV
            return "";
#else
            string[] guids = AssetDatabase.FindAssets("t:" + _type);
            foreach (string g in guids) {
                string p = AssetDatabase.GUIDToAssetPath(g);
                if (p.EndsWith(_file)) {
                    return p;
                }
            }
            return "";
#endif
        }

        private void SetEditorIDFA() {
            string editorIdfa = EditorPrefs.GetString(EditorPrefEditorIDFA);
            if (string.IsNullOrEmpty(editorIdfa)) {
                editorIdfa = Guid.NewGuid().ToString();
                EditorPrefs.SetString(EditorPrefEditorIDFA, editorIdfa);
            }

            sdkBundleSettings.EditorIdfa = editorIdfa;
        }

        private void SaveChange() {
            RemoveEmptyStr();
            EditorUtility.SetDirty(sdkBundleSettings);
            AssetDatabase.SaveAssets();
        }

        private void RemoveEmptyStr() {
            if (sdkBundleSettings==null) {
                return;
            }
            sdkBundleSettings.gameAnalyticsAndroidGameKey = sdkBundleSettings.gameAnalyticsAndroidGameKey.Replace(" ", "");
            sdkBundleSettings.gameAnalyticsAndroidSecretKey = sdkBundleSettings.gameAnalyticsAndroidSecretKey.Replace(" ", "");
            sdkBundleSettings.gameAnalyticsIosGameKey = sdkBundleSettings.gameAnalyticsIosGameKey.Replace(" ", "");
            sdkBundleSettings.gameAnalyticsIosSecretKey = sdkBundleSettings.gameAnalyticsIosSecretKey.Replace(" ", "");
            sdkBundleSettings.facebookAppId = sdkBundleSettings.facebookAppId.Replace(" ", "");
            sdkBundleSettings.adjustIOSToken = sdkBundleSettings.adjustIOSToken.Replace(" ", "");
            sdkBundleSettings.adjustAndroidToken = sdkBundleSettings.adjustAndroidToken.Replace(" ", "");
        }
    }

}