#set page(fill: black)
#set text(fill: white)

<EOF>
#context {
  let q = query(<EOF>)
  assert(q.len() > 0 and q.first().location().page() <= 2, message: "Mais de 2 páginas.")
}
