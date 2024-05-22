

using ReferenceApi.Employees;
using System.Collections;

namespace ReferenceApi.ContractTests.Employees;
public class EmployeesSampleData : IEnumerable<object[]>
{
    private readonly IReadOnlyList<object[]> _data = [
        [
            new EmployeeCreateRequest
            {
                FirstName = "Robert",
                LastName = "Smith"
            },
            "smith-robert"
            ],
            [
                new EmployeeCreateRequest
                {
                    FirstName = "Johnny",
                    LastName = "Marr"
                },
                "marr-johnny"
            ],
            [
               new EmployeeCreateRequest
                {
                    FirstName = "Johnny",
                    LastName = "Marr"
                },
                "marr-johnny-a"
                ]
        ];

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}
